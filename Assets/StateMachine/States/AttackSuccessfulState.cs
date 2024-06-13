using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSuccessfulState : IState
{
    private PlayerController player;
    public AttackSuccessfulState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        Debug.Log("Entering Attack Sucessful State");
        if (player.UnitManager.NoPlayerUnitsWithActionsLeft())
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.playerToEnemyTurnState);
        }
        else
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
        }
    }
    public void Exit()
    {
        player.PlayerStateMachine.PreviousState = this;
    }
    public void Update()
    {
        //
    }
}
