using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerUnit : BattleUnit
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private BattleUnitInfo playerUnitInfo;
    [SerializeField] private GameObject movementTile;
    [SerializeField] private GameObject attackTile;
    [SerializeField] private GameObject movementRangeHolder;
    [SerializeField] private GameObject unitBattleStatsHolder;
    [SerializeField] private TMP_Text unitBattleStatsText;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarHolder;


    private float healthStat;
    private float maxHealthStat;
    private float attackStat;
    private float movementStat;
    private HashSet<Vector3> allCrossableTilePositionsInMovementRange;
    private HashSet<Vector3> allTilePositionsInMovementRange;
    private HashSet<Vector3> allTilePositionsInAttackRange;
    private BattleStats baseStats;
    private BattleStats currentStats;

    // Every tile in movement range taking obstacles into consideration
    public override HashSet<Vector3> AllCrossableTilePositionsInMovementRange { get => allCrossableTilePositionsInMovementRange; set => allCrossableTilePositionsInMovementRange = value; }

    // Every tile in movement range ignoring all obstacles
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
    public override BattleUnitInfo BattleUnitInfo { get => playerUnitInfo; set => playerUnitInfo = value; }
    public override BattleStats BaseStats { get => baseStats; set => baseStats = value; }
    public override BattleStats CurrentStats { get => currentStats; set => currentStats = value; }
    
    private void Awake() { }
    private void OnDestroy() { }
}
