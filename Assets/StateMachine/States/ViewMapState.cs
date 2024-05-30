using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ViewMapState : IState
{
    private PlayerController player;
    private PlayerUnit playerUnit;

    public bool selectUnitActionState;
    //public event Action<float> AttackTargetSelected;
    //public event Action PlayerUnitSelected;

    private bool selectUnitState;
    private bool hoverState;
    private float timer;
    private float timeoutLength;
    private Vector3 playerDummyOriginalPosition;
    public ViewMapState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.1f;
    }
    public void Enter()
    {
        Debug.Log("Entering ViewMapState");
    }
    public void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            HandlePlayerMovement();
        }
        HandleUnitSelection();
    }
    public void Exit()
    {
        Debug.Log("Exiting ViewMapState");
    }
    private void HandlePlayerMovement()
    {
        if (selectUnitActionState) return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            player.transform.Translate(Vector2.up);
            HoverOverUnit();
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.Translate(Vector2.left);
            HoverOverUnit();
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            player.transform.Translate(Vector2.down);
            HoverOverUnit();
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.Translate(Vector2.right);
            HoverOverUnit();
            timer = timeoutLength;
        }
    }
    private void CheckValidPosition(Vector2 direction)
    {
        if (!selectUnitState || playerUnit == null) return;

        // Add (0, 0, 1) because cursor is has z position of -1, while all valid positions have z position of 0
        if (playerUnit.validPositions.Contains(player.transform.position + new Vector3(0, 0, 1))) return;

        player.transform.Translate(direction * -1);
    }
    private void HoverOverUnit()
    {
        Collider2D col = Physics2D.OverlapPoint(player.transform.position, LayerMask.GetMask("Player Unit"));
        if (col != null && !hoverState)
        {
            playerUnit = col.gameObject.GetComponent<PlayerUnit>();
            playerUnit.ToggleMovementRangeVisibility();
            playerDummyOriginalPosition = player.transform.position;
            hoverState = true;
        }
        if (col == null && hoverState)
        {
            playerUnit.ToggleMovementRangeVisibility();
            hoverState = false;
        }
    }
    private void HandleUnitSelection()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Collider2D col = Physics2D.OverlapPoint(player.transform.position, LayerMask.GetMask("Player Unit"));
            if (col != null)
            {
                //selectUnitState = true;
                //selectUnitActionState = true;
                playerUnit = col.gameObject.GetComponent<PlayerUnit>();
                playerDummyOriginalPosition = player.transform.position;
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.selectUnitActionState);
            }

            // If a player unit is currently selected
            //else
            //{
            //    Collider2D col = Physics2D.OverlapPoint(player.transform.position, LayerMask.GetMask("Enemy Unit"));
            //    if (col != null)
            //    {
            //        Debug.Log("Hello");
            //        float enemyAttackStat = col.gameObject.GetComponent<TestDummy>().attackStat;
            //        AttackTargetSelected?.Invoke(enemyAttackStat);
            //    }
            //    CancelUnitSelection();

            //}
        }
        //if (Input.GetKeyDown(KeyCode.X) && selectUnitState) CancelUnitSelection();
    }

    // Cancels unit selection if the x key was pressed or no valid target was selected for attack
    private void CancelUnitSelection()
    {
        selectUnitState = false;
        selectUnitActionState = false;
        hoverState = false;
        //PlayerUnitSelected?.Invoke();
        playerUnit.ToggleMovementRangeVisibility();
    }
}
