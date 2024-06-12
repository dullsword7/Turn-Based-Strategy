using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class Stat
{
    public StatName statName;
    public float statValue;
    public float statGrowth;
}

public enum StatName
{
    Level,
    Health,
    Attack,
    Defense,
    Speed,
    Movement
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BattleStatsScriptableObject", order = 1)]
public class BattleUnitInfo : ScriptableObject
{
    public string BattleUnitName;
    public float EXPValueOnKill;

    public List<Stat> BattleStatsList;

}

