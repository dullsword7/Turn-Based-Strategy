using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectAttackTargetState : IState
{
    private PlayerController player;

    private float timer;
    private float timeoutLength;
    private EnemyUnit enemyUnit;
    private bool lockControls;

    private EnemyUnit previousEnemyUnit;
    public SelectAttackTargetState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;
    }

    public void Enter()
    {
        Debug.Log("Entering SelectAttackTargetState");
        lockControls = false;
    }

    public void Update ()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (lockControls) return;
            HandlePlayerMovement();
        }

        if (lockControls) return;
        if (Input.GetKeyDown(KeyCode.X)) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.selectUnitActionState);
        if (Input.GetKeyDown(KeyCode.Z)) AttackTargetSelected();
    }

    public void Exit()
    {
        enemyUnit?.TurnOffInfo();
        lockControls = false;
        player.BattleResultHandler.TurnOffBattleResultCanvas();

        player.PlayerStateMachine.PreviousState = this;
    }

    /// <summary>
    /// Handles player input.
    /// </summary>
    private void HandlePlayerMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            player.transform.Translate(Vector2.up);
            CheckValidPosition(Vector2.up);
            HoverOverUnit();
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.Translate(Vector2.left);
            CheckValidPosition(Vector2.left);
            HoverOverUnit();
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            player.transform.Translate(Vector2.down);
            CheckValidPosition(Vector2.down);
            HoverOverUnit();
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.Translate(Vector2.right);
            CheckValidPosition(Vector2.right);
            HoverOverUnit();
            timer = timeoutLength;
        }
    }

    /// <summary>
    /// Checks if the player input will be outside the select unit's attack range. If it is, then undo the movement.
    /// </summary>
    /// <param name="direction">the opposite direction to send the player cursor</param>
    private void CheckValidPosition(Vector2 direction)
    {
        if (player.PlayerUnit == null) return;

        if (player.PlayerUnit.AllTilePositionsInAttackRange.Contains(player.transform.position)) return;

        player.transform.Translate(direction * -1);
    }

    /// <summary>
    /// When an enemy unit has been select for an attack, attacks the enemy. 
    /// </summary>
    private void AttackTargetSelected()
    {
        Collider2D col = Physics2D.OverlapPoint(player.transform.position, LayerMask.GetMask("Enemy Unit"));
        if (col != null)
        {
            EnemyUnit enemy = col.GetComponent<EnemyUnit>();
            lockControls = true;
            player.PlayerUnit.TurnOffMovementRange();
            player.PlayerUnit.TurnOffInfo();

            player.StartCoroutine(AttackEnemyUnit(enemy));
        }
    }

    /// <summary>
    /// Moves to the enemy unit, plays the start up attack animation, plays the skills sprite, and deals damage to the enemy,
    /// then marks the attacking unit's turn as completed.
    /// </summary>
    /// <param name="enemy">the enemy being attacked</param>
    /// <returns></returns>
    private IEnumerator AttackEnemyUnit(EnemyUnit enemy)
    {
        yield return player.PlayerUnit.StartCoroutine(player.PlayerUnit.TryMoveToAttackPosition(enemy.transform.position));
        if (!player.PlayerUnit.TryMovementSucess) yield break;

        for (int i = 0; i < player.BattleResultHandler.DetermineNumberOfAttacks(); i++)
        {
            if (player.UnitManager.enemyUnitList.Contains(enemy))
            {
                yield return player.PlayerUnit.StartCoroutine(player.PlayerUnit.StartAndWaitForAnimation("PlayerUnitScream"));
                Vector3 direction = enemy.transform.position - player.PlayerUnit.transform.position;
                SpriteFactory.Instance.InstantiateSkillSprite("Slash", enemy.transform.position, direction);
                yield return enemy.StartCoroutine(enemy.ReceiveDamage(player.BattleResultHandler.DetermineDamageToTarget(), player.PlayerUnit));
            } 
        }

        player.PlayerUnit.UnitHasCompletedAllActions();

        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.attackSuccessfulState); 
        yield return null;
    }

    /// <summary>
    /// Handles displaying enemy info when hoving them.
    /// </summary>
    private void HoverOverUnit()
    {
        Collider2D col = Physics2D.OverlapPoint(player.transform.position, Constants.MASK_ENEMY_UNIT);

        if (col != null)
        {
            TurnOffPreviousUnitInfo();
            enemyUnit = col.gameObject.GetComponent<EnemyUnit>();
            enemyUnit.TurnOnInfo();

            player.BattleResultHandler.SetCurrentDefendingUnit(enemyUnit);
            player.BattleResultHandler.TurnOnBattleResultCanvas();

            previousEnemyUnit = enemyUnit;
        }
        if (col == null)
        {
            enemyUnit?.TurnOffInfo();
            enemyUnit?.TurnOffMovementRange();
            player.BattleResultHandler.TurnOffBattleResultCanvas();
        }
    }

    /// <summary>
    /// Handles the case when hovering over a unit, an adjacent unit is hovered over. 
    /// </summary>
    private void TurnOffPreviousUnitInfo()
    {
        previousEnemyUnit?.TurnOffInfo();
        previousEnemyUnit?.TurnOffMovementRange();
    }
}

