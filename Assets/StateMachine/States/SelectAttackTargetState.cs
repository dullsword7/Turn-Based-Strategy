using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAttackTargetState : IState
{
    private PlayerController player;

    private float timer;
    private float timeoutLength;
    private bool hoverState;
    private TestDummy enemyUnit;
    public SelectAttackTargetState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;
    }
    public void Enter()
    {
        Debug.Log("Entering SelectAttackTargetState");
    }
    public void Update ()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            HandlePlayerMovement();
        }

        if (Input.GetKeyDown(KeyCode.X)) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.selectUnitActionState);
        if (Input.GetKeyDown(KeyCode.Z)) AttackTargetSelected();
    }
    public void Exit()
    {
        enemyUnit.ToggleInfoVisibility();
        hoverState = false;
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
            col.GetComponent<TestDummy>().RecieveDamage(player.PlayerUnit.attackStat);
            SpriteFactory.Instance.InstantiateSkillSprite("Slash", col.transform.position);
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
        }
    }
    private void HoverOverUnit()
    {
        Collider2D col = Physics2D.OverlapPoint(player.transform.position, LayerMask.GetMask("Enemy Unit"));

        if (col != null && !hoverState)
        {
            enemyUnit = col.gameObject.GetComponent<TestDummy>();
            enemyUnit.ToggleInfoVisibility();
            hoverState = true;
        }
        if (col == null && hoverState)
        {
            enemyUnit.ToggleInfoVisibility();
            hoverState = false;
        }
    }
}

