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

    BattleUnit battleUnit;
    BattleUnit previousBattleUnit;
    public ViewMapState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;
    }

    // Entering this state represents the start of a new turn
    public void Enter()
    {
        Debug.Log("Entering ViewMapState");

        if (player.PlayerStateMachine.PreviousState != player.PlayerStateMachine.selectUnitActionState)
        {
            player.PlayerUnit?.TurnOffMovementRange();
            player.PlayerUnit?.TurnOffInfo();
        }

        Collider2D col = Physics2D.OverlapPoint(player.transform.position, Constants.MASK_BATTLE_UNIT);
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

        foreach (PlayerUnit playerUnit in player.UnitManager.playerUnitList)
        {
            playerUnit.UpdateAttackAndMovementRange(playerUnit.transform.position);
        }
        foreach (EnemyUnit enemyUnit in player.UnitManager.enemyUnitList)
        {
            enemyUnit.UpdateAttackAndMovementRange(enemyUnit.transform.position);
        }

        if (player.UnitManager.NoPlayerUnitsWithActionsLeft())
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.playerToEnemyTurnState);
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
    private void HoverOverUnit()
    {
        Collider2D col = Physics2D.OverlapPoint(player.transform.position, Constants.MASK_BATTLE_UNIT);
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

    /// <summary>
    /// Pressing Z selects the Player unit at the current position.
    /// </summary>
    private void HandleUnitSelection()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Collider2D col = Physics2D.OverlapPoint(player.transform.position, Constants.MASK_PLAYER_UNIT);
            if (col != null)
            {
                playerUnit = col.gameObject.GetComponent<PlayerUnit>();
                if (playerUnit.noMoreActions) return;
                player.BattleResultHandler.SetCurrentAttackingUnit(playerUnit);
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
    private void CheckValidPosition(Vector2 direction)
    {
        if (!player.CheckCursorInBounds()) player.transform.Translate(direction * -1);
    }
}
