using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerUnit : MonoBehaviour, IBattleUnit
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private BattleStatsScriptableObject unitBattleStats;
    [SerializeField] private GameObject validTile;
    [SerializeField] private GameObject movementRangeHolder;
    [SerializeField] private GameObject unitBattleStatsCanvas;
    [SerializeField] private TextMeshProUGUI unitBattleStatsText;

    public float healthStat;
    public float attackStat;
    public float movementStat;
    public HashSet<Vector3> validPositions;

    public void DealDamage(float damageDealt)
    {
        healthStat -= damageDealt;
        Debug.Log("Dealing: " + damageDealt);
    }

    public void ToggleMovementRangeVisibility()
    {
        if (movementRangeHolder.activeInHierarchy)
        {
            movementRangeHolder.SetActive(false);
            unitBattleStatsCanvas.SetActive(false);
        }
        else
        {
            movementRangeHolder.SetActive(true);
            unitBattleStatsCanvas.SetActive(true);
        }
    }

    public void InitializeMovementRange(Vector3 startPosition)
    {
        startPosition = new Vector3(startPosition.x, startPosition.y, 0);
        validPositions = initializeValidPositions(startPosition);
        calculateValidMovementPositions(1, validPositions);
        foreach (Vector3 position in validPositions)
        {
            Instantiate(validTile, position, Quaternion.identity, movementRangeHolder.transform);
        }
    }
    
    public void InitalizeBattleStats()
    {
        healthStat = unitBattleStats.healthStat;
        attackStat = unitBattleStats.attackStat;
        movementStat = unitBattleStats.movementStat;

        string battleStatsString = $"Health: {healthStat} {Environment.NewLine} Attack: {attackStat} {Environment.NewLine} Move: {movementStat}";
        unitBattleStatsText.SetText(battleStatsString);
    }

    public void Start()
    {
        InitalizeBattleStats();
        InitializeMovementRange(transform.position);
        movementRangeHolder.SetActive(false);
    }
    private void Awake()
    {
        if (playerInput != null)
        {
            playerInput.AttackTargetSelected += DealDamage;
            playerInput.PlayerUnitSelected += ToggleMovementRangeVisibility;
        }
    }
    private void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.AttackTargetSelected -= DealDamage;
            playerInput.PlayerUnitSelected -= ToggleMovementRangeVisibility;
        }
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
        if (counter == movementStat)
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
