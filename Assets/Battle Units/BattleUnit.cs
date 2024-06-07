using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BattleUnit : MonoBehaviour, IBattleUnit
{
    private const string animBaseLayer = "Base Layer";
    private int playerUnitScream = Animator.StringToHash(animBaseLayer + ".PlayerUnitScream");
    private int enemyUnitScream = Animator.StringToHash(animBaseLayer + ".EnemyUnitScream");

    public Action<GameObject> BattleUnitDeath;

    public abstract HashSet<Vector3> AllTilePositionsInMovementRange { get; set; }
    public abstract HashSet<Vector3> AllTilePositionsInAttackRange { get; set; }
    public abstract HashSet<Vector3> ValidMovementPositions { get; set; }
    public abstract GameObject UnitBattleStatsHolder { get; set; }
    public abstract GameObject HealthBarHolder { get; set; }
    public abstract Image HealthBar { get; set; }
    public abstract float MaxHealthStat { get; set; }
    public abstract float HealthStat { get; set; }
    public abstract float AttackStat { get; set; }
    public abstract float MovementStat { get; set; }
    public abstract TMP_Text UnitBattleStatsText { get; set; }
    public abstract void InitalizeBattleStats();

    public virtual List<Vector3> ShowMovementPath(Vector3 endPosition) 
    {
        Dictionary<Vector3, List<Vector3>> graph = new Dictionary<Vector3, List<Vector3>>();
        graph = Helpers.ValidMovementPositionsToAdjacencyList(transform.position, ValidMovementPositions);
        List<Vector3> path = Helpers.BFS(graph, transform.position, endPosition);
        foreach (Vector3 position in path)
        {
            SpriteFactory.Instance.InstantiateSkillSprite("Movement Path", position, Vector3.zero);
        }
        return path;
    }
    public virtual Vector3 ClosestValidAttackPosition(Vector3 attackTargetPosition)
    {
        List<Vector3> attackPositions = new List<Vector3>();
        Vector3 pos1 = attackTargetPosition + Vector3.up;
        Vector3 pos2 = attackTargetPosition + Vector3.down;
        Vector3 pos3 = attackTargetPosition + Vector3.left;
        Vector3 pos4 = attackTargetPosition + Vector3.right;

        // if the attacker is already adjacent to its target, dont move
        if (transform.position == pos1 || transform.position == pos2 || transform.position == pos3 || transform.position == pos4) return transform.position;

        if (ValidMovementPositions.Contains(pos1)) attackPositions.Add(pos1);
        if (ValidMovementPositions.Contains(pos2)) attackPositions.Add(pos2);
        if (ValidMovementPositions.Contains(pos3)) attackPositions.Add(pos3);
        if (ValidMovementPositions.Contains(pos4)) attackPositions.Add(pos4);

        if (attackPositions.Count == 0) return attackTargetPosition;

        return FindClosestPosition(attackPositions);
    }
    public virtual IEnumerator MoveToPosition(Vector3 attackTargetPosition, Action onComplete = null)
    {
        InitializeMovementRange(transform.position);
        Vector3 targetDestination = ClosestValidAttackPosition(attackTargetPosition);

        List<Vector3> path = ShowMovementPath(targetDestination);

        // should return immediately if there is no path
        if (path.Count < 1)
        {
            Debug.Log("No path to destination found, canceling movement");
            yield return null;
        } 


        Vector3 direction = targetDestination - transform.position;
        Vector3 xTargetDestination = new Vector3(targetDestination.x, transform.position.y, transform.position.z);
        Vector3 startingPosition = transform.position;
         
        float elapsedTime = 0;

        float xTimer = Math.Abs(direction.x) / 2;
        float yTimer = Math.Abs(direction.y) / 2;
        // move horizontally first
        while (elapsedTime < xTimer)
        {
            transform.position = Vector3.Lerp(startingPosition, xTargetDestination, (elapsedTime / xTimer));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = xTargetDestination;
        startingPosition = transform.position;
        elapsedTime = 0;

        // move vertically
        while (elapsedTime < yTimer)
        {
            transform.position = Vector3.Lerp(startingPosition, targetDestination, (elapsedTime / yTimer));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetDestination;


        yield return new WaitForSeconds(1f);
        onComplete?.Invoke();
    }
    public IEnumerator StartAndWaitForAnimation(string stateName, Action onComplete = null)
    {
        Animator anim = GetComponent<Animator>();

        //Get hash of animation
        int animHash = 0;
        if (stateName == "PlayerUnitScream")
            animHash = playerUnitScream;
        if (stateName == "EnemyUnitScream")
            animHash = enemyUnitScream;

         //targetAnim.Play(stateName);
        anim.CrossFadeInFixedTime(stateName, 0.6f);

        //Wait until we enter the current state
        while (anim.GetCurrentAnimatorStateInfo(0).fullPathHash != animHash)
        {
            yield return null;
        }

        float counter = 0;
        float waitTime = anim.GetCurrentAnimatorStateInfo(0).length;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Done playing. Do something below!
        Debug.Log("Done Playing");
        yield return new WaitForSeconds(.5f);
        onComplete?.Invoke();

    }
    public void TurnOffInfo()
    {
        UnitBattleStatsHolder.SetActive(false);
        HealthBarHolder.SetActive(false);
    }
    public void TurnOnInfo()
    {
        UnitBattleStatsHolder.SetActive(true);
        HealthBarHolder.SetActive(true);
    }
    public virtual IEnumerator ReceiveDamage(float damageAmount, BattleUnit battleUnit, Action onComplete = null)
    {
        float healthBeforeDamage = HealthStat;
        float healthAfterDamage = HealthStat - damageAmount;
        HealthStat -= damageAmount;
        if (healthAfterDamage <= 0)
        {
            HealthStat = 0;
            healthAfterDamage = 0;
        }

        while (healthBeforeDamage > healthAfterDamage)
        {
            healthBeforeDamage -= 1;
            UpdateStats(healthBeforeDamage, battleUnit);
            HealthBar.fillAmount = healthBeforeDamage / MaxHealthStat;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Finished Updating Healthbar");
        yield return new WaitForSeconds(2f);
        onComplete?.Invoke();
        if (HealthStat <= 0) BattleUnitDeath?.Invoke(gameObject);
    }
    public void UpdateStats(float currentHealth, BattleUnit battleUnit)
    {
        string battleStatsString = "No Stats Found";
        if (battleUnit is PlayerUnit)
        {
            battleStatsString = $"Player {Environment.NewLine} HP: {currentHealth} / {MaxHealthStat} {Environment.NewLine} ATK: {AttackStat} {Environment.NewLine} MOV: {MovementStat}";
        }
        if (battleUnit is EnemyUnit)
        {
            battleStatsString = $"Enemy {Environment.NewLine} HP: {currentHealth} / {MaxHealthStat} {Environment.NewLine} ATK: {AttackStat} {Environment.NewLine} MOV: {MovementStat}";
        }
        UnitBattleStatsText.SetText(battleStatsString);
    }
    public void InitializeMovementRange(Vector3 startPosition)
    {
        startPosition = new Vector3(startPosition.x, startPosition.y, 0);
        AllTilePositionsInMovementRange = initializeValidPositions(startPosition);
        calculateValidMovementPositions(1, AllTilePositionsInMovementRange);

    }
    public HashSet<Vector3> FindInvalidPositions()
    {
        HashSet<Vector3> invalidPositions = new HashSet<Vector3>();
        foreach (Vector3 position in AllTilePositionsInMovementRange)
        {
            Collider2D col = Physics2D.OverlapPoint(position, Constants.MASK_BATTLE_UNIT);
            if (col != null) invalidPositions.Add(position);
        }
        return invalidPositions;
    }
    // Creates a set of the startingPosition unioned with its four adjacent tiles, if they are not occupied
    private HashSet<Vector3> initializeValidPositions(Vector3 startingPosition)
    {
        HashSet<Vector3> res = new HashSet<Vector3>();
        res.Add(startingPosition);

        if (MovementStat == 0) return res;

        Vector3 pos1 = startingPosition + Vector3.up;
        Vector3 pos2 = startingPosition + Vector3.down;
        Vector3 pos3 = startingPosition + Vector3.left;
        Vector3 pos4 = startingPosition + Vector3.right;

        if (CheckTilePositionEmpty(pos1)) res.Add(pos1);
        if (CheckTilePositionEmpty(pos2)) res.Add(pos2);
        if (CheckTilePositionEmpty(pos3)) res.Add(pos3);
        if (CheckTilePositionEmpty(pos4)) res.Add(pos4);

        return res;
    }
    private void calculateValidMovementPositions(int counter, HashSet<Vector3> validPositions)
    {
        HashSet<Vector3> newValidPositions = new HashSet<Vector3>();
        if (counter >= MovementStat)
        {
            AllTilePositionsInAttackRange = calculateTilesInAttackRange(validPositions);
            return;
        }
        else
        {
            foreach (Vector3 position in validPositions)
            {
                Vector3 pos1 = position + Vector3.up;
                Vector3 pos2 = position + Vector3.down;
                Vector3 pos3 = position + Vector3.left;
                Vector3 pos4 = position + Vector3.right;

                if (CheckTilePositionEmpty(pos1)) newValidPositions.Add(pos1);
                if (CheckTilePositionEmpty(pos2)) newValidPositions.Add(pos2);
                if (CheckTilePositionEmpty(pos3)) newValidPositions.Add(pos3);
                if (CheckTilePositionEmpty(pos4)) newValidPositions.Add(pos4);
            }

            AllTilePositionsInMovementRange.UnionWith(newValidPositions);
            calculateValidMovementPositions(counter + 1, newValidPositions);
        } 
    }

    /// <summary>
    /// Calculates a BattleUnit's attack tiles.
    /// </summary>
    /// <param name="positions">the tiles of a BattleUnit's movement range</param>
    /// <returns>HashSet containing the tile positions a BattleUnit can attack</returns>
    private HashSet<Vector3> calculateTilesInAttackRange(HashSet<Vector3> positions)
    {
        Debug.Log(positions.Count);
        HashSet<Vector3> attackableTiles = new HashSet<Vector3>(positions);
        foreach (Vector3 position in positions)
        {
            Vector3 pos1 = position + Vector3.up;
            Vector3 pos2 = position + Vector3.down;
            Vector3 pos3 = position + Vector3.left;
            Vector3 pos4 = position + Vector3.right;

            attackableTiles.Add(pos1);
            attackableTiles.Add(pos2);
            attackableTiles.Add(pos3);
            attackableTiles.Add(pos4);

        }
        return attackableTiles;
    }
    private Vector3 FindClosestPosition(List<Vector3> validAttackPositions)
    {
        Vector3 currentPosition = transform.position;
        float smallestDistance = float.MaxValue;
        Vector3 closestPosition = new Vector3();
        foreach (Vector3 position in validAttackPositions)
        {
            float distance = Vector3.Distance(currentPosition, position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestPosition = position;
            }
        }
        return closestPosition;
    }
    private bool CheckTilePositionEmpty(Vector3 position)
    {
        Collider2D col = Physics2D.OverlapPoint(position, Constants.MASK_BATTLE_UNIT);
        return col == null;
    }
}
