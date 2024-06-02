using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour, IBattleUnit
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

    private HashSet<Vector3> validPositions;

    public bool updatingHealthBar;
    public void Start()
    {
        InitalizeBattleStats();
        InitializeMovementRange(transform.position);
    }
    public void InitalizeBattleStats()
    {
        healthStat = testDummyStats.healthStat;
        maxHealthStat = healthStat;
        attackStat = testDummyStats.attackStat;
        movementStat = testDummyStats.movementStat;

        string battleStatsString = $"Health: {healthStat} / {maxHealthStat} {Environment.NewLine} Attack: {attackStat} {Environment.NewLine}";
        unitBattleStatsText.SetText(battleStatsString);
    }
    public void TurnOnInfo()
    {
        unitBattleStatsHolder.SetActive(true); 
        healthBarHolder.SetActive(true); 
    }
    public void TurnOffInfo()
    {
        unitBattleStatsHolder.SetActive(false);
        healthBarHolder.SetActive(false); 
    }
    public IEnumerator RecieveDamge(float damageAmount, Action onComplete = null)
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
        string battleStatsString = $"Health: {currentHealth} / {maxHealthStat} {Environment.NewLine} Attack: {attackStat} {Environment.NewLine}";
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
