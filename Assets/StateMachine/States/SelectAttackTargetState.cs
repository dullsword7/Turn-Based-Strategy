using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectAttackTargetState : IState
{
    private PlayerController player;

    private float timer;
    private float timeoutLength;
    private bool hoverState;
    private EnemyUnit enemyUnit;
    private bool lockControls;
    private Action waitForHealthBars;
    public SelectAttackTargetState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;
        waitForHealthBars += HealthBarsFinishedUpdating;
    }
    public void Enter()
    {
        Debug.Log("Entering SelectAttackTargetState");
        hoverState = false;
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
        hoverState = false;
        lockControls = false;

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

        if (player.PlayerUnit.validPositions.Contains(player.transform.position)) return;

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
            player.PlayerUnit.StartCoroutine(player.PlayerUnit.MoveToPosition(enemy.transform.position, () => {
                player.PlayerUnit.StartCoroutine(player.PlayerUnit.StartAndWaitForAnimation("PlayerUnitScream", () => {
                    Vector3 direction = enemy.transform.position - player.PlayerUnit.transform.position;
                    enemy.StartCoroutine(enemy.ReceiveDamage(player.PlayerUnit.AttackStat, enemy, waitForHealthBars));
                    SpriteFactory.Instance.InstantiateSkillSprite("Slash", col.transform.position, direction);
                }));
            }));
        }
    }
    private void HoverOverUnit()
    {
        Collider2D col = Physics2D.OverlapPoint(player.transform.position, LayerMask.GetMask("Enemy Unit"));

        if (col != null && !hoverState)
        {
            enemyUnit = col.gameObject.GetComponent<EnemyUnit>();
            enemyUnit.TurnOnInfo();
            hoverState = true;
        }
        if (col == null && hoverState)
        {
            enemyUnit.TurnOffInfo();
            hoverState = false;
        }
    }

    private void HealthBarsFinishedUpdating()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.attackSuccessfulState); 
    }
}

