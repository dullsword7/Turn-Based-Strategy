using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BattleStatsScriptableObject", order = 1)]
public class BattleStatsScriptableObject : ScriptableObject
{
    public float healthStat;
    public float attackStat;
    public float movementStat;
}
