using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerUnit : BattleUnit
{
    [SerializeField] private BattleUnitInfo playerUnitInfo;
    [SerializeField] private GameObject movementTile;
    [SerializeField] private GameObject attackTile;
    [SerializeField] private GameObject movementRangeHolder;
    [SerializeField] private GameObject unitBattleStatsHolder;
    [SerializeField] private TMP_Text unitBattleStatsText;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarHolder;
    [SerializeField] private Image expBar;

    private float maxHealthStat;
    private HashSet<Vector3> allTilePositionsInMovementRange;
    private HashSet<Vector3> allTilePositionsInAttackRange;

    private float currentExp;
    private float expToNextLevel;

    public override HashSet<Vector3> AllTilePositionsInMovementRange { get => allTilePositionsInMovementRange; set => allTilePositionsInMovementRange = value; }
    public override HashSet<Vector3> AllTilePositionsInAttackRange { get => allTilePositionsInAttackRange; set => allTilePositionsInAttackRange = value; }
    public override GameObject UnitBattleStatsHolder { get => unitBattleStatsHolder; set => unitBattleStatsHolder = value; }
    public override GameObject MovementRangeHolder { get => movementRangeHolder; set => movementRangeHolder = value; }
    public override GameObject HealthBarHolder { get => healthBarHolder; set => healthBarHolder = value; }
    public override Image HealthBar { get => healthBar; set => healthBar = value; }
    public override float MaxHealthStat { get => maxHealthStat; set => maxHealthStat = value; }
    public override TMP_Text UnitBattleStatsText { get => unitBattleStatsText; set => unitBattleStatsText = value; }
    public override GameObject MovementTile { get => movementTile; set => movementTile = value; }
    public override GameObject AttackTile { get => attackTile; set => attackTile = value; }
    public override BattleUnitInfo BattleUnitInfo { get => playerUnitInfo; set => playerUnitInfo = value; }


    public EXPHandler expHandler;

    public float CurrentExp { get => currentExp; set => currentExp = value; }
    public float ExpToNextLevel { get => expToNextLevel; set => expToNextLevel = value; }
    public Image ExpBar { get => expBar; set => expBar = value; }

    public GameObject UnitActionsPanel;
    public List<GameObject> UnitActionMenuButtons;

    public bool noMoreActions;


    private void Start()
    {
        expHandler = new EXPHandler(this);
        expToNextLevel = Constants.PLAYER_UNIT_EXP_TO_FIRST_LEVEL;
    }

    private void OnDestroy() { }

    /// <summary>
    /// Makes the game object inactive once their health has reached 0.
    /// </summary>
    /// <param name="attackingBattleUnit">the BattleUnit that dealt the finishing blow</param>
    public override IEnumerator HandleBattleUnitDeath(BattleUnit attackingBattleUnit)
    {
        yield return null;
    }

    /// <summary>
    /// Completes an individual player unit's turn.
    /// </summary>
    public void UnitHasCompletedAllActions()
    {
        noMoreActions = true;
        this.ChangeColorToIndicateBattleUnitTurnOver();
    }
}
