using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBattlePhaseState : IState
{
    private PlayerController player;
    private List<EnemyUnit> enemies;
    private Action waitForHealthBarsToFinishUpdating;

    public EnemyBattlePhaseState(PlayerController player)
    {
        this.player = player;
        waitForHealthBarsToFinishUpdating += HealthBarsFinishedUpdating;
    }
    public void Enter()
    {
        Debug.Log("Entering Enemy Battle Phase State");

        foreach (EnemyUnit enemy in player.UnitManager.enemyUnitList)
        {
            if (enemy.IsPlayerUnitInRange(player.PlayerUnit))
            {
                Debug.Log("Attacking player unit");
                AttackTarget(enemy, enemy.attackStat);
            }
        }
        // if all enemies are dead or if player is dead, return
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
        enemyUnit.StartCoroutine(enemyUnit.MoveToPosition(player.PlayerUnit.transform.position, () => EnemyUnitFinishedMoving(enemyUnit, damage)));
    }
    private void EnemyUnitFinishedMoving(EnemyUnit enemyUnit, float damage)
    {
        enemyUnit.StartCoroutine(enemyUnit.StartAndWaitForAnimation("EnemyUnitScream", () => AnimationFinishedPlaying(enemyUnit, damage)));
    }
    private void AnimationFinishedPlaying(EnemyUnit enemyUnit, float damage)
    {
        Vector3 direction = player.PlayerUnit.transform.position - enemyUnit.transform.position;
        SpriteFactory.Instance.InstantiateSkillSprite("Slash", player.PlayerUnit.transform.position, direction);
        player.PlayerUnit.StartCoroutine(player.PlayerUnit.ReceiveDamage(damage, player.PlayerUnit, waitForHealthBarsToFinishUpdating));
    }
    private void HealthBarsFinishedUpdating()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.enemyToPlayerTurnState); 
    }

}
