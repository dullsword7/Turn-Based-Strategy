using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BattleStatsScriptableObject", order = 1)]
public class BattleStatsScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct BattleStats
    {
        public int Health;
        public int Attack;
        public int Movement;
    }
    public string Name;

    public BattleStats baseStats;

    //public float healthStat;
    //public float attackStat;
    //public float movementStat;


}

