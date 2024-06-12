using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBattlePhaseState : IState
{
    private PlayerController player;
    private List<EnemyUnit> enemies;
    private Action waitForAllEnemyAttacksToFinish;

    public EnemyBattlePhaseState(PlayerController player)
    {
        this.player = player;
        waitForAllEnemyAttacksToFinish += AllEnemyAttacksCompleted;
    }
    public void Enter()
    {
        Debug.Log("Entering Enemy Battle Phase State");

        player.StartCoroutine(ProcessEnemyUnitAttacks(waitForAllEnemyAttacksToFinish));
    }

    public void Exit()
    {
        Debug.Log("Exiting Enemy Battle Phase State");
    }
    private void AllEnemyAttacksCompleted()
    {
        if (player.UnitManager.AllPlayerUnitsDead())
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
        }
        else
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.enemyToPlayerTurnState);
        }
    }
    private IEnumerator ProcessEnemyUnitAttacks(Action onComplete)
    {
        foreach (EnemyUnit enemy in player.UnitManager.enemyUnitList)
        {
            if (player.UnitManager.playerUnitList.Count != 0)
            {
                Debug.Log("Attacking player unit");
                player.BattleResultHandler.SetCurrentAttackingUnit(enemy);
                player.BattleResultHandler.SetCurrentDefendingUnit(player.PlayerUnit);
                player.BattleResultHandler.UpdateBattleResultCanvas();
                yield return player.StartCoroutine(AttackTarget(enemy));
                enemy.ChangeColorToIndicateBattleUnitTurnOver();
            }
        }
        onComplete?.Invoke();
        yield return null;
    }
    private IEnumerator AttackTarget(EnemyUnit enemyUnit)
    {
        player.PlayerUnit.TurnOffMovementRange();
        player.PlayerUnit.TurnOnInfo();
        yield return enemyUnit.StartCoroutine(enemyUnit.TryMoveToPosition(player.PlayerUnit.transform.position));
        if (!enemyUnit.TryMovementSucess) yield break;

        // if the playerUnit is not within enemyUnit's attack range
        if (!enemyUnit.IsPlayerUnitInRange(player.PlayerUnit)) yield break;

        player.BattleResultHandler.TurnOnBattleResultCanvas();
        for (int i = 0; i < player.BattleResultHandler.DetermineNumberOfAttacks(); i++)
        {
            if (player.UnitManager.playerUnitList.Contains(player.PlayerUnit))
            {
                yield return enemyUnit.StartCoroutine(enemyUnit.StartAndWaitForAnimation("EnemyUnitScream"));
                Vector3 direction = player.PlayerUnit.transform.position - enemyUnit.transform.position;
                SpriteFactory.Instance.InstantiateSkillSprite("Slash", player.PlayerUnit.transform.position, direction);
                yield return player.PlayerUnit.StartCoroutine(player.PlayerUnit.ReceiveDamage(player.BattleResultHandler.DetermineDamageToTarget(), enemyUnit));
            }

        }
        player.BattleResultHandler.TurnOffBattleResultCanvas();
    }
    public void Update() { }
}
