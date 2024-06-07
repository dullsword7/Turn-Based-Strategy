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

    public BattleStatsScriptableObject testDummyStats;
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
    public override float MaxHealthStat { get => maxHealthStat; set => maxHealthStat = value; }
    public override float HealthStat { get => healthStat; set => healthStat = value; }
    public override float AttackStat { get => attackStat; set => attackStat = value; }
    public override float MovementStat { get => movementStat; set => movementStat = value; }
    public override TMP_Text UnitBattleStatsText { get => unitBattleStatsText; set => unitBattleStatsText = value; }
    public override GameObject MovementTile { get => movementTile; set => movementTile = value; }
    public override GameObject AttackTile { get => attackTile; set => attackTile = value; }
    public void Start()
    {
        InitalizeBattleStats();
        InitializeAttackAndMovementRange(transform.position);
        SetUpMovementRangeIndicator();
        SetUpAttackRangeIndicator();
        movementRangeHolder.SetActive(false);
    }
    public override void InitalizeBattleStats()
    {
        healthStat = testDummyStats.baseStats.Health;
        maxHealthStat = healthStat;
        attackStat = testDummyStats.baseStats.Attack;
        movementStat = testDummyStats.baseStats.Movement;

        string battleStatsString = $"Enemy {Environment.NewLine} HP: {healthStat} / {maxHealthStat} {Environment.NewLine} ATK: {attackStat} {Environment.NewLine}";
        unitBattleStatsText.SetText(battleStatsString);
    }
    public bool IsPlayerUnitInRange(PlayerUnit player)
    {
        return allTilePositionsInAttackRange.Contains(player.transform.position);
    }
}
