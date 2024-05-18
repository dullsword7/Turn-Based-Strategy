using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour, IBattleUnit
{
    [SerializeField] private PlayerInput playerInput;
    public BattleStatsScriptableObject playerDummyStats;
    public float healthStat;
    public float attackStat;

    public BattleStatsScriptableObject UnitBattleStats { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void DealDamage(float damageDealt)
    {
        healthStat -= damageDealt;
        Debug.Log("Dealing: " + damageDealt);
    }

    public void InitalizeBattleStats()
    {
        throw new System.NotImplementedException();
    }

    public void Start()
    {
        healthStat = playerDummyStats.healthStat;
        attackStat = playerDummyStats.attackStat;
    }
    private void Awake()
    {
        if (playerInput != null)
        {
            playerInput.AttackTargetSelected += DealDamage;
        }
    }
    private void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.AttackTargetSelected -= DealDamage;
        }
    }
}
