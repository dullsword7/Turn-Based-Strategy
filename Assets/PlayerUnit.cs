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
    [SerializeField] private GameObject validTile;
    [SerializeField] private GameObject movementRangeHolder;
    [SerializeField] private GameObject unitBattleStatsHolder;
    [SerializeField] private TMP_Text unitBattleStatsText;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarHolder;


    private float healthStat;
    private float maxHealthStat;
    private float attackStat;
    private float movementStat;
    private HashSet<Vector3> validPositions;
    private HashSet<Vector3> validMovementPositions;

    public override HashSet<Vector3> ValidPositions { get => validPositions; set => validPositions = value; }
    public override HashSet<Vector3> ValidMovementPositions { get => validMovementPositions; set => validMovementPositions = value; }
    public override GameObject UnitBattleStatsHolder { get => unitBattleStatsHolder; set => unitBattleStatsHolder = value; }
    public override GameObject HealthBarHolder { get => healthBarHolder; set => healthBarHolder = value; }
    public override Image HealthBar { get => healthBar; set => healthBar = value; }
    public override float MaxHealthStat { get => maxHealthStat; set => maxHealthStat = value; }
    public override float HealthStat { get => healthStat; set => healthStat = value; }
    public override float AttackStat { get => attackStat; set => attackStat = value; }
    public override float MovementStat { get => movementStat; set => movementStat = value; }
    public override TMP_Text UnitBattleStatsText { get => unitBattleStatsText; set => unitBattleStatsText = value; }
    public void DealDamage(float damageDealt)
    {
        healthStat -= damageDealt;
        Debug.Log("Dealing: " + damageDealt);
    }
    
    public void TurnOnMovementRange()
    {
        movementRangeHolder.SetActive(true);
    }
    public void TurnOffMovementRange()
    {
        movementRangeHolder.SetActive(false);
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
    private void SetUpMovementRangeIndicator()
    {
        foreach (Vector3 position in validPositions)
        {
            Instantiate(validTile, position, Quaternion.identity, movementRangeHolder.transform);
        }
    }
    public void Start()
    {
        InitalizeBattleStats();
        InitializeMovementRange(transform.position);
        SetUpMovementRangeIndicator();
        movementRangeHolder.SetActive(false);
        ShowMovementPath();
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
