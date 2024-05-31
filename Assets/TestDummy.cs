using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour, IBattleUnit
{
    public BattleStatsScriptableObject testDummyStats;
    public float healthStat;
    public float attackStat;
    public void Start()
    {
        healthStat = testDummyStats.healthStat;
        attackStat = testDummyStats.attackStat;
    }

    public void DealDamage(float damageAmount)
    {
    }

    public void InitalizeBattleStats()
    {

    }
    public void RecieveDamage(float damageAmount)
    {
        healthStat -= damageAmount;
    }

    //public void applyDamage(PlayerUnit playerDummy)
    //{
    //    healthStat -= playerDummy.attackStat; 
    //}
}
