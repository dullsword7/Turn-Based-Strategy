using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TestDummy : MonoBehaviour, IBattleUnit
{
    [SerializeField] TextMeshProUGUI unitBattleStatsText;
    [SerializeField] GameObject unitBattleStatsCanvas;

    public BattleStatsScriptableObject testDummyStats;
    public float healthStat;
    public float maxHealthStat;
    public float attackStat;
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
    public void RecieveDamage(float damageAmount)
    {
        healthStat -= damageAmount;
        UpdateStats();
    }
    public void UpdateStats()
    {
        string battleStatsString = $"Health: {healthStat} / {maxHealthStat} {Environment.NewLine} Attack: {attackStat} {Environment.NewLine}";
        unitBattleStatsText.SetText(battleStatsString);
    }
    public void ToggleInfoVisibility()
    {
        if (unitBattleStatsCanvas.activeInHierarchy)
        {
            unitBattleStatsCanvas.SetActive(false);
        }
        else
        {
            unitBattleStatsCanvas.SetActive(true);
        }
    }

    //public void applyDamage(PlayerUnit playerDummy)
    //{
    //    healthStat -= playerDummy.attackStat; 
    //}
}
