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

    EnemyUnit previousEnemyUnit;
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
    private void CheckValidPosition(Vector2 direction)
    {
        if (player.PlayerUnit == null) return;

        if (player.PlayerUnit.AllTilePositionsInAttackRange.Contains(player.transform.position)) return;

        player.transform.Translate(direction * -1);
    }
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
    private IEnumerator AttackEnemyUnit(EnemyUnit enemy)
    {
        yield return player.PlayerUnit.StartCoroutine(player.PlayerUnit.TryMoveToPosition(enemy.transform.position));
        yield return player.PlayerUnit.StartCoroutine(player.PlayerUnit.StartAndWaitForAnimation("PlayerUnitScream"));

        Vector3 direction = enemy.transform.position - player.PlayerUnit.transform.position;
        SpriteFactory.Instance.InstantiateSkillSprite("Slash", enemy.transform.position, direction);
        yield return enemy.StartCoroutine(enemy.ReceiveDamage(player.PlayerUnit.BattleUnitStats[StatName.Attack], player.PlayerUnit));
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.attackSuccessfulState); 
        yield return null;
    }
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

    // Handles the case when hovering over a unit, an adjacent unit is hovered over. 
    private void TurnOffPreviousUnitInfo()
    {
        previousEnemyUnit?.TurnOffInfo();
        previousEnemyUnit?.TurnOffMovementRange();
    }
}

