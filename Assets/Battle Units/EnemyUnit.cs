using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class EnemyUnit : BattleUnit
{
    [SerializeField] private TMP_Text unitBattleStatsText;
    [SerializeField] private GameObject unitBattleStatsHolder;
    [SerializeField] private GameObject movementRangeHolder;
    [SerializeField] private GameObject healthBarHolder;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject movementTile;
    [SerializeField] private GameObject attackTile;

    public BattleUnitInfo enemyUnitInfo;
    private float currentLevel;
    private float healthStat;
    private float maxHealthStat;
    private float attackStat;
    private float movementStat;

    private HashSet<Vector3> allTilePositionsInMovementRange;
    private HashSet<Vector3> allTilePositionsInAttackRange;
    public override HashSet<Vector3> AllTilePositionsInMovementRange { get => allTilePositionsInMovementRange; set => allTilePositionsInMovementRange = value; }
    public override HashSet<Vector3> AllTilePositionsInAttackRange { get => allTilePositionsInAttackRange; set => allTilePositionsInAttackRange = value; }
    public override GameObject UnitBattleStatsHolder { get => unitBattleStatsHolder; set => unitBattleStatsHolder = value; }
    public override GameObject MovementRangeHolder { get => movementRangeHolder; set => movementRangeHolder = value; }
    public override GameObject HealthBarHolder { get => healthBarHolder; set => healthBarHolder = value; }
    public override Image HealthBar { get => healthBar; set => healthBar = value; }

    public override float CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public override float MaxHealthStat { get => maxHealthStat; set => maxHealthStat = value; }
    public override float HealthStat { get => healthStat; set => healthStat = value; }
    public override float AttackStat { get => attackStat; set => attackStat = value; }
    public override float MovementStat { get => movementStat; set => movementStat = value; }
    public override TMP_Text UnitBattleStatsText { get => unitBattleStatsText; set => unitBattleStatsText = value; }
    public override GameObject MovementTile { get => movementTile; set => movementTile = value; }
    public override GameObject AttackTile { get => attackTile; set => attackTile = value; }
    public override BattleUnitInfo BattleUnitInfo { get => enemyUnitInfo; set => enemyUnitInfo = value; }

    /// <summary>
    /// Checks if a PlayerUnit is in attack range.
    /// </summary>
    /// <param name="player">the PlayerUnit</param>
    /// <returns>true if the player is in range, false otherwise</returns>
    public bool IsPlayerUnitInRange(PlayerUnit player)
    {
        return allTilePositionsInAttackRange.Contains(player.transform.position);
    }

    /// <summary>
    /// Makes the game object inactive once their health has reached 0.
    /// </summary>
    /// <param name="attackingBattleUnit">the BattleUnit that dealt the finishing blow</param>
    public override IEnumerator HandleBattleUnitDeath(BattleUnit attackingBattleUnit)
    {
        PlayerUnit playerUnit = attackingBattleUnit as PlayerUnit;
        yield return StartCoroutine(playerUnit.expHandler.UpdateExp(playerUnit, BattleUnitInfo.EXPValueOnKill));
    }
}
