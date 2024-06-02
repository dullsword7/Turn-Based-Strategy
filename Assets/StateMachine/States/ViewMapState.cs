using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ViewMapState : IState
{
    private PlayerController player;
    private PlayerUnit playerUnit;

    private bool selectUnitState;
    private bool hoverState;
    private float timer;
    private float timeoutLength;
    public ViewMapState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;
    }
    public void Enter()
    {
        Debug.Log("Entering ViewMapState");
        if (player.PlayerStateMachine.PreviousState == player.PlayerStateMachine.selectUnitActionState)
        {
            hoverState = true;
        }
        else
        {
            hoverState = false;
        }
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

        player.PlayerStateMachine.PreviousState = this;
    }
    private void HandlePlayerMovement()
    {
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
    private void HoverOverUnit()
    {
        Collider2D col = Physics2D.OverlapPoint(player.transform.position, LayerMask.GetMask("Player Unit"));

        if (col != null && !hoverState)
        {
            playerUnit = col.gameObject.GetComponent<PlayerUnit>();
            player.PlayerUnit = playerUnit;
            playerUnit.TurnOnInfo();
            hoverState = true;
        }
        if (col == null && hoverState)
        {
            playerUnit.TurnOffInfo();
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
                playerUnit = col.gameObject.GetComponent<PlayerUnit>();
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.selectUnitActionState);
            }
        }
    }
}
