using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBattlePhaseState : IState
{
    private PlayerController player;
    private List<EnemyUnit> enemies;
    private Action waitForHealthBars;

    public EnemyBattlePhaseState(PlayerController player)
    {
        this.player = player;
        waitForHealthBars += HealthBarsFinishedUpdating;
    }
    public void Enter()
    {
        Debug.Log("Entering Enemy Battle Phase State");
        // if all enemies are dead or if player is dead, return

        foreach (EnemyUnit enemy in player.EnemyUnitList)
        {
            if (enemy.IsPlayerUnitInRange(player.PlayerUnit))
            {
                Debug.Log("Attacking player unit");
                AttackTarget(enemy.attackStat);
            }
            // do enemy behavior
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Enemy Battle Phase State");
    }

    public void Update()
    {
    }
    private void AttackTarget(float damage)
    {
        player.PlayerUnit.TurnOffMovementRange();
        player.PlayerUnit.TurnOnInfo();
        SpriteFactory.Instance.InstantiateSkillSprite("Slash", player.PlayerUnit.transform.position);
        player.PlayerUnit.StartCoroutine(player.PlayerUnit.RecieveDamge(damage, waitForHealthBars));
    }
    private void HealthBarsFinishedUpdating()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState); 
    }

}
