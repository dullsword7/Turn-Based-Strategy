using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ViewMapState : IState
{
    private PlayerController player;
    private PlayerUnit playerUnit;

    private float timer;
    private float timeoutLength;

    private LayerMask layerMask;
    BattleUnit battleUnit;
    BattleUnit previousBattleUnit;
    public ViewMapState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;
        
        // allows raycast to target both "Player Unit" layer and "Enemy Unit" layer
        layerMask = (1 << LayerMask.NameToLayer("Player Unit") | 1 << LayerMask.NameToLayer("Enemy Unit"));
    }

    // Entering this state represents the start of a new turn
    public void Enter()
    {
        Debug.Log("Entering ViewMapState");
        player.PlayerUnit?.TurnOffMovementRange();
        player.PlayerUnit?.TurnOffInfo();

        foreach (BattleUnit battleUnit in player.UnitManager.enemyUnitList)
        {
            battleUnit.RestoreBattleUnitOriginalColor();
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
        Collider2D col = Physics2D.OverlapPoint(player.transform.position, layerMask);
        if (col != null)
        {
            TurnOffPreviousUnitInfo();
            battleUnit = col.gameObject.GetComponent<BattleUnit>();
            battleUnit.TurnOnInfo();
            battleUnit.TurnOnMovementRange();

            if (battleUnit is PlayerUnit)
            {
                playerUnit = col.gameObject.GetComponent<PlayerUnit>();
                player.PlayerUnit = playerUnit;
                playerUnit.TurnOnMovementRange();
                playerUnit.TurnOnInfo();
            }
            previousBattleUnit = battleUnit;
        }
        if (col == null)
        {
            battleUnit?.TurnOffInfo();
            battleUnit?.TurnOffMovementRange();

            if (battleUnit is PlayerUnit)
            {
                playerUnit.TurnOffMovementRange();
                playerUnit.TurnOffInfo();
            }
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
    // Handles the case when hovering over a unit, an adjacent unit is hovered over. 
    private void TurnOffPreviousUnitInfo()
    {
        previousBattleUnit?.TurnOffInfo();
        previousBattleUnit?.TurnOffMovementRange();
        if (previousBattleUnit is PlayerUnit)
        {
            PlayerUnit playerUnit = previousBattleUnit as PlayerUnit;
            playerUnit.TurnOffMovementRange();
        }
    }
}
