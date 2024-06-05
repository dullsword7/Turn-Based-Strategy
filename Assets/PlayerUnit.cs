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
    [SerializeField] private TextMeshProUGUI unitBattleStatsText;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarHolder;


    public float healthStat;
    public float maxHealthStat;
    public float attackStat;
    public float movementStat;
    public Action PlayerUnitDeath;

    public override HashSet<Vector3> ValidPositions { get => validPositions; set => validPositions = value; }
    public HashSet<Vector3> validPositions;

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
    public void TurnOffInfo()
    {
        unitBattleStatsHolder.SetActive(false);
        healthBarHolder.SetActive(false);
    }
    public void TurnOnInfo()
    {
        unitBattleStatsHolder.SetActive(true);
        healthBarHolder.SetActive(true);
    }

    public void InitializeMovementRange(Vector3 startPosition)
    {
        startPosition = new Vector3(startPosition.x, startPosition.y, 0);
        validPositions = initializeValidPositions(startPosition);
        calculateValidMovementPositions(1, validPositions);
        foreach (Vector3 position in validPositions)
        {
            Instantiate(validTile, position, Quaternion.identity, movementRangeHolder.transform);
        }
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
    public override IEnumerator ReceiveDamage(float damageAmount, Action onComplete = null)
    {
        float healthBeforeDamage = healthStat;
        float healthAfterDamage = healthStat - damageAmount;
        healthStat -= damageAmount;
        if (healthAfterDamage <= 0)
        {
            healthStat = 0;
            healthAfterDamage = 0;
        }

        while (healthBeforeDamage > healthAfterDamage)
        {
            healthBeforeDamage -= 1;
            UpdateStats(healthBeforeDamage);
            healthBar.fillAmount = healthBeforeDamage / maxHealthStat;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Finished Updating Healthbar");
        yield return new WaitForSeconds(2f);
        onComplete?.Invoke();
        if (healthStat <= 0) PlayerUnitDeath?.Invoke();
    }
    public void UpdateStats(float currentHealth)
    {
        string battleStatsString = $"Player {Environment.NewLine} HP: {currentHealth} / {maxHealthStat} {Environment.NewLine} ATK: {attackStat} {Environment.NewLine} MOV: {movementStat}";
        unitBattleStatsText.SetText(battleStatsString);
    }

    public void Start()
    {
        InitalizeBattleStats();
        InitializeMovementRange(transform.position);
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

    private HashSet<Vector3> initializeValidPositions(Vector3 startingPosition)
    {
        HashSet<Vector3> res = new HashSet<Vector3>();
        res.Add(startingPosition);
        res.Add(startingPosition + Vector3.up);
        res.Add(startingPosition + Vector3.down);
        res.Add(startingPosition + Vector3.left);
        res.Add(startingPosition + Vector3.right);
        return res;
    }
    public void calculateValidMovementPositions(int counter, HashSet<Vector3> validPositions)
    {
        HashSet<Vector3> newValidPositions = new HashSet<Vector3>();
        if (counter >= movementStat)
        {
            return;
        }
        else
        {
            foreach (Vector3 position in validPositions)
            {
                newValidPositions.Add(position + Vector3.up);
                newValidPositions.Add(position + Vector3.down);
                newValidPositions.Add(position + Vector3.left);
                newValidPositions.Add(position + Vector3.right);
            }
            validPositions.UnionWith(newValidPositions);
            calculateValidMovementPositions(counter + 1, validPositions);
        } 
    }
}
