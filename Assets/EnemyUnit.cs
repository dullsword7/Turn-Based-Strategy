using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class EnemyUnit : BattleUnit
{
    [SerializeField] private TextMeshProUGUI unitBattleStatsText;
    [SerializeField] private GameObject unitBattleStatsHolder;
    [SerializeField] private GameObject healthBarHolder;
    [SerializeField] private Image healthBar;

    public BattleStatsScriptableObject testDummyStats;
    public float healthStat;
    public float maxHealthStat;
    public float attackStat;
    public float movementStat;


    public override HashSet<Vector3> ValidPositions { get => validPositions; set => validPositions = value; }
    private HashSet<Vector3> validPositions;

    public override GameObject UnitBattleStatsHolder { get => unitBattleStatsHolder; set => unitBattleStatsHolder = value; }
    public override GameObject HealthBarHolder { get => healthBarHolder; set => healthBarHolder = value; }

    public void Start()
    {
        InitalizeBattleStats();
        InitializeMovementRange(transform.position);
    }
    public override void InitalizeBattleStats()
    {
        healthStat = testDummyStats.healthStat;
        maxHealthStat = healthStat;
        attackStat = testDummyStats.attackStat;
        movementStat = testDummyStats.movementStat;

        string battleStatsString = $"Enemy {Environment.NewLine} HP: {healthStat} / {maxHealthStat} {Environment.NewLine} ATK: {attackStat} {Environment.NewLine}";
        unitBattleStatsText.SetText(battleStatsString);
    }
    public override IEnumerator ReceiveDamage(float damageAmount, Action onComplete = null)
    {
        float healthBeforeDamage = healthStat;
        float healthAfterDamage = healthStat - damageAmount;
        healthStat -= damageAmount;

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
    }
    public void UpdateStats(float currentHealth)
    {
        string battleStatsString = $"Enemy {Environment.NewLine} HP: {currentHealth} / {maxHealthStat} {Environment.NewLine} ATK: {attackStat} {Environment.NewLine}";
        unitBattleStatsText.SetText(battleStatsString);
    }
    public bool IsPlayerUnitInRange(PlayerUnit player)
    {
        return validPositions.Contains(player.transform.position);
    }
    public void InitializeMovementRange(Vector3 startPosition)
    {
        startPosition = new Vector3(startPosition.x, startPosition.y, 0);
        validPositions = initializeValidPositions(startPosition);
        calculateValidMovementPositions(1, validPositions);
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
