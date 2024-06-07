using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerUnit : BattleUnit
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private BattleStatsScriptableObject unitBattleStats;
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
    public void DealDamage(float damageDealt)
    {
        healthStat -= damageDealt;
        Debug.Log("Dealing: " + damageDealt);
    }
    
    
    public override void InitalizeBattleStats()
    {
        healthStat = unitBattleStats.healthStat;
        maxHealthStat = healthStat;
        attackStat = unitBattleStats.attackStat;
        movementStat = unitBattleStats.movementStat;

        string battleStatsString = $"Player {Environment.NewLine} HP: {healthStat} / {maxHealthStat} {Environment.NewLine} ATK: {attackStat} {Environment.NewLine} MOV: {movementStat}";
        unitBattleStatsText.SetText(battleStatsString);
    }
    public void Start()
    {
        InitalizeBattleStats();
        InitializeMovementRange(transform.position);
        SetUpMovementRangeIndicator();
        SetUpAttackRangeIndicator();
        movementRangeHolder.SetActive(false);
    }
    private void Awake()
    {
        if (playerInput != null)
        {
            playerInput.AttackTargetSelected += DealDamage;
        }
    }
    private void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.AttackTargetSelected -= DealDamage;
        }
    }
}
