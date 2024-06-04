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

        foreach (GameObject enemy in player.EnemyUnitList)
        {
            EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
            if (enemyUnit.IsPlayerUnitInRange(player.PlayerUnit))
            {
                Debug.Log("Attacking player unit");
                AttackTarget(enemyUnit, enemyUnit.attackStat);
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
    private void AttackTarget(EnemyUnit enemyUnit, float damage)
    {
        player.PlayerUnit.TurnOffMovementRange();
        player.PlayerUnit.TurnOnInfo();
        enemyUnit.StartCoroutine(enemyUnit.MoveToPosition(player.PlayerUnit.transform.position, () => {
            Vector3 direction = player.PlayerUnit.transform.position - enemyUnit.transform.position;
            SpriteFactory.Instance.InstantiateSkillSprite("Slash", player.PlayerUnit.transform.position, direction);
            player.PlayerUnit.StartCoroutine(player.PlayerUnit.RecieveDamge(damage, waitForHealthBars));
        }));
    }
    private void HealthBarsFinishedUpdating()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.enemyToPlayerTurnState); 
    }

}
