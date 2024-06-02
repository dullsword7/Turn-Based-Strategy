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
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.enemyBattlePhaseState);
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
