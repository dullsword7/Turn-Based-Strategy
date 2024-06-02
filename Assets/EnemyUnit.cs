using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour, IBattleUnit
{
    [SerializeField] TextMeshProUGUI unitBattleStatsText;
    [SerializeField] GameObject unitBattleStatsCanvas;
    [SerializeField] Image healthBar;

    public BattleStatsScriptableObject testDummyStats;
    public float healthStat;
    public float maxHealthStat;
    public float attackStat;

    public bool updatingHealthBar;
    public void Start()
    {
        InitalizeBattleStats();
    }

    public void DealDamage(float damageAmount)
    {
    }

    public void InitalizeBattleStats()
    {
        healthStat = testDummyStats.healthStat;
        maxHealthStat = healthStat;
        attackStat = testDummyStats.attackStat;

        string battleStatsString = $"Health: {healthStat} / {maxHealthStat} {Environment.NewLine} Attack: {attackStat} {Environment.NewLine}";
        unitBattleStatsText.SetText(battleStatsString);
    }
    public void TurnOnInfo()
    {
        unitBattleStatsCanvas.SetActive(true); 
    }
    public void TurnOffInfo()
    {
        unitBattleStatsCanvas.SetActive(false);
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
        yield return new WaitForSeconds(0.5f);
        onComplete?.Invoke();
    }
    public void UpdateStats(float currentHealth)
    {
        string battleStatsString = $"Health: {currentHealth} / {maxHealthStat} {Environment.NewLine} Attack: {attackStat} {Environment.NewLine}";
        unitBattleStatsText.SetText(battleStatsString);
    }
}
