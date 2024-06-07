using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BattleStats
{
    public int Health;
    public int Attack;
    public int Movement;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BattleStatsScriptableObject", order = 1)]
public class BattleUnitInfo : ScriptableObject
{
    public string BattleUnitName;

    public BattleStats baseStats;
}

