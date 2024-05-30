using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    public BattleStatsScriptableObject testDummyStats;
    public float healthStat;
    public float attackStat;
    public void Start()
    {
        healthStat = testDummyStats.healthStat;
        attackStat = testDummyStats.attackStat;
    }
    public void applyDamage(PlayerUnit playerDummy)
    {
        healthStat -= playerDummy.attackStat; 
    }
}
