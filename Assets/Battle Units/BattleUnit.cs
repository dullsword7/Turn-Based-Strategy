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
    public abstract GameObject UnitBattleStatsHolder { get; set; }
    public abstract GameObject MovementRangeHolder { get; set; }
    public abstract GameObject HealthBarHolder { get; set; }
    public abstract Image HealthBar { get; set; }

    public abstract float MaxHealthStat { get; set; }

    public abstract TMP_Text UnitBattleStatsText { get; set; }
    public abstract GameObject MovementTile { get; set; }
    public abstract GameObject AttackTile { get; set; }
    public abstract BattleUnitInfo BattleUnitInfo { get; set; }
    public virtual List<Stat> BattleUnitBaseStats { get; set; }
    public virtual Dictionary<StatName, float> BattleUnitStats { get; set; }
    public virtual Dictionary<StatName, float> BattleUnitStatsGrowthRates { get; set; }

    public bool TryMovementSucess;

    public void Awake()
    {
        InitializeBattleStats();
        UpdateAttackAndMovementRange(transform.position);
        MovementRangeHolder.SetActive(false);

    }
    /// <summary>
    /// Reads and sets stats from BattleStats struct in BattleUnitInfo
    /// </summary>
    public void InitializeBattleStats()
    {
        BattleUnitStats = new Dictionary<StatName, float>();
        BattleUnitStatsGrowthRates = new Dictionary<StatName, float>();
        BattleUnitBaseStats = BattleUnitInfo.BattleStatsList;

        foreach (Stat stat in BattleUnitInfo.BattleStatsList)
        {
            BattleUnitStats.Add(stat.statName, stat.statValue);
        }
        foreach (Stat stat in BattleUnitInfo.BattleStatsList)
        {
            BattleUnitStatsGrowthRates.Add(stat.statName, stat.statGrowth);
        }

        MaxHealthStat = BattleUnitStats[StatName.Health];

        UpdateStats();
    }

    /// <summary>
    /// Makes BattleUnit's movement range indicator visible.
    /// </summary>
    public void TurnOnMovementRange()
    {
        MovementRangeHolder.SetActive(true);
    }

    /// <summary>
    /// Makes BattleUnit's movement range indicator no longer visible?
    /// </summary>
    public void TurnOffMovementRange()
    {
        MovementRangeHolder.SetActive(false);
    }

    /// <summary>
    ///  Instantiates the AttackTile prefab at every position in BattleUnit's attack range.
    /// </summary>
    public void SetUpAttackRangeIndicator()
    {
        foreach (Vector3 position in AllTilePositionsInAttackRange)
        {
            Instantiate(AttackTile, position, Quaternion.identity, MovementRangeHolder.transform);
        }
    }

    /// <summary>
    /// Instantiates the MovementTile prefab at every position in BattleUnit's movement range.
    /// </summary>
    public void SetUpMovementRangeIndicator()
    {
        foreach (Vector3 position in AllTilePositionsInMovementRange)
        {
            Instantiate(MovementTile, position, Quaternion.identity, MovementRangeHolder.transform);
        }
    }

    /// <summary>
    /// Destroys all children of the MovementRangeHolder
    /// </summary>
    public void ClearMovementRangeIndicator()
    {
        foreach (Transform child in MovementRangeHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Updates the visuals of a BattleUnit's attack and movement range.
    /// </summary>
    private void UpdateAttackAndMovementRangeIndicator()
    {
        ClearMovementRangeIndicator();
        SetUpMovementRangeIndicator();
        SetUpAttackRangeIndicator();
    }

    /// <summary>
    /// Displays a BattleUnit's caculated movement path from its current position, to a target position.
    /// </summary>
    /// <param name="endPosition">the final movement position</param>
    /// <returns>a list of positions of the shortest path to the target position</returns>
    public virtual List<Vector3> CalculateMovementPath(Vector3 endPosition) 
    {
        calculateValidMovementPositions(1, initializeValidPositions(transform.position));
        Dictionary<Vector3, List<Vector3>> graph = new Dictionary<Vector3, List<Vector3>>();
        graph = Helpers.ValidMovementPositionsToAdjacencyList(transform.position, AllTilePositionsInMovementRange);
        List<Vector3> path = Helpers.BFS(graph, transform.position, endPosition);

        //foreach (Vector3 position in AllTilePositionsInMovementRange)
        //{
        //    SpriteFactory.Instance.InstantiateSkillSprite("Movement Path", position, Vector3.zero);
        //}
        foreach (Vector3 position in path)
        {
            SpriteFactory.Instance.InstantiateSkillSprite("Movement Path", position, Vector3.zero);
        }

        return path;
    }

    /// <summary>
    /// Calculates the closest valid attack position, from the target to the attacking BattleUnit.   
    /// </summary>
    /// <param name="attackTargetPosition">the position of BattleUnit being targeted</param>
    /// <returns>the location of the closest valid attack position</returns>
    public Vector3 ClosestValidAttackPosition(Vector3 attackTargetPosition)
    {
        List<Vector3> attackPositions = new List<Vector3>();
        Vector3 pos1 = attackTargetPosition + Vector3.up;
        Vector3 pos2 = attackTargetPosition + Vector3.down;
        Vector3 pos3 = attackTargetPosition + Vector3.left;
        Vector3 pos4 = attackTargetPosition + Vector3.right;

        // if the attacker is already adjacent to its target, dont move
        if (transform.position == pos1 || transform.position == pos2 || transform.position == pos3 || transform.position == pos4) return transform.position;

        if (AllTilePositionsInMovementRange.Contains(pos1) && CheckTilePositionContainsAllyUnit(pos1)) attackPositions.Add(pos1);
        if (AllTilePositionsInMovementRange.Contains(pos2) && CheckTilePositionContainsAllyUnit(pos2)) attackPositions.Add(pos2);
        if (AllTilePositionsInMovementRange.Contains(pos3) && CheckTilePositionContainsAllyUnit(pos3)) attackPositions.Add(pos3);
        if (AllTilePositionsInMovementRange.Contains(pos4) && CheckTilePositionContainsAllyUnit(pos4)) attackPositions.Add(pos4);

        if (attackPositions.Count == 0) return attackTargetPosition;

        return FindClosestPosition(attackPositions);
    }
    /// <summary>
    /// Creates a list of the positions around the attack target that are both in range and empty. 
    /// </summary>
    /// <param name="attackTargetPosition">the position of the target being attacked</param>
    /// <returns>a list of positions the attacking unit can use</returns>
    public List<Vector3> AllValidAttackPosition(Vector3 attackTargetPosition)
    {
        List<Vector3> attackPositions = new List<Vector3>();
        Vector3 pos1 = attackTargetPosition + Vector3.up;
        Vector3 pos2 = attackTargetPosition + Vector3.down;
        Vector3 pos3 = attackTargetPosition + Vector3.left;
        Vector3 pos4 = attackTargetPosition + Vector3.right;

        // if the attacker is already adjacent to its target, dont move
        if (transform.position == pos1 || transform.position == pos2 || transform.position == pos3 || transform.position == pos4)
        {
            attackPositions.Add(transform.position);
            return attackPositions;
        }

        if (AllTilePositionsInMovementRange.Contains(pos1) && CheckNoBattleUnitOnTile(pos1)) attackPositions.Add(pos1);
        if (AllTilePositionsInMovementRange.Contains(pos2) && CheckNoBattleUnitOnTile(pos2)) attackPositions.Add(pos2);
        if (AllTilePositionsInMovementRange.Contains(pos3) && CheckNoBattleUnitOnTile(pos3)) attackPositions.Add(pos3);
        if (AllTilePositionsInMovementRange.Contains(pos4) && CheckNoBattleUnitOnTile(pos4)) attackPositions.Add(pos4);

        return attackPositions;
    }

    /// <summary>
    /// Checks for a valid position around the attack target, if one is found then move there.
    /// </summary>
    /// <param name="attackTargetPosition">the position of the target</param>
    /// <param name="onComplete">action invoked when coroutine completes</param>
    /// <returns></returns>
    public virtual IEnumerator TryMoveToAttackPosition(Vector3 attackTargetPosition, Action onComplete = null)
    {
        UpdateAttackAndMovementRange(transform.position);

        List<Vector3> validPositions = AllValidAttackPosition(attackTargetPosition);

        Vector3 targetPosition = transform.position;
        foreach (Vector3 position in validPositions)
        {
            Collider2D col = Physics2D.OverlapPoint(position, Constants.MASK_BATTLE_UNIT);
            if (col == null)
            {
                targetPosition = position;
            }
        }

        List<Vector3> path = CalculateMovementPath(targetPosition);

        // should return immediately if there is no path
        if (path.Count < 1 || validPositions.Count < 1)
        {
            TryMovementSucess = false;
            Debug.Log("No path to destination found, canceling movement");
            yield return new WaitForSeconds(1f);
            yield break;
        }

        TryMovementSucess = true;

        for (int i = 0; i < path.Count - 1; i++)
        {
            yield return StartCoroutine(MoveAlongPath(path[i], path[i + 1]));
        }

        yield return new WaitForSeconds(1f);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Checks for a valid position around the attack target, if one is found then move there.
    /// </summary>
    /// <param name="targetDestination">the position of the target</param>
    /// <param name="onComplete">action invoked when coroutine completes</param>
    /// <returns></returns>
    public virtual IEnumerator TryMoveToPosition(Vector3 targetDestination)
    {
        UpdateAttackAndMovementRange(transform.position);

        List<Vector3> path = CalculateMovementPath(targetDestination);

        // should return immediately if there is no path
        if (path.Count < 1)
        {
            TryMovementSucess = false;
            Debug.Log("No path to destination found, canceling movement");
            yield return new WaitForSeconds(1f);
            yield break;
        }

        TryMovementSucess = true;

        for (int i = 0; i < path.Count - 1; i++)
        {
            yield return StartCoroutine(MoveAlongPath(path[i], path[i + 1]));
        }

        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// BattleUnit will lerp its positions from one position to another.
    /// </summary>
    /// <param name="startPosition">the start position</param>
    /// <param name="endPosition">the end position</param>
    /// <returns></returns>
    private IEnumerator MoveAlongPath(Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0;
        float timer = 0.5f;

        // move vertically
        while (elapsedTime < timer)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / timer));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
    }

    /// <summary>
    /// Starts an animation and returns once the animation is complete.
    /// </summary>
    /// <param name="stateName">name of the animation to start and wait for</param>
    /// <param name="onComplete">action invoked when coroutine completes</param>
    /// <returns></returns>
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

    /// <summary>
    /// Turns off BattleUnit's battle stats and health bar.
    /// </summary>
    public void TurnOffInfo()
    {
        UnitBattleStatsHolder.SetActive(false);
        HealthBarHolder.SetActive(false);
    }

    /// <summary>
    /// Turns on BattleUnit's battle stats and health bar.
    /// </summary>
    public void TurnOnInfo()
    {
        UnitBattleStatsHolder.SetActive(true);
        HealthBarHolder.SetActive(true);
    }

    /// <summary>
    /// BattleUnit receives damage, updates their hp values, and health bar visual.
    /// </summary>
    /// <param name="damageAmount">the amount of damage to recieve</param>
    /// <param name="attackingBattleUnit">the BattleUnit dealing the damage</param>
    /// <returns></returns>
    public virtual IEnumerator ReceiveDamage(float damageAmount, BattleUnit attackingBattleUnit)
    {
        float healthBeforeDamage = BattleUnitStats[StatName.Health];
        float healthAfterDamage = BattleUnitStats[StatName.Health] - damageAmount;
        if (healthAfterDamage <= 0)
        {
            BattleUnitStats[StatName.Health] = 0;
            healthAfterDamage = 0;
        }
        BattleUnitStats[StatName.Health] -= damageAmount;

        yield return StartCoroutine(UpdateHealthBar(healthBeforeDamage, healthAfterDamage));

        Debug.Log("Finished Updating Healthbar");
        yield return new WaitForSeconds(1f);
        if (BattleUnitStats[StatName.Health] <= 0) 
        {
            yield return StartCoroutine(HandleBattleUnitDeath(attackingBattleUnit));
            BattleUnitDeath?.Invoke(gameObject);
        }
    }
    
    /// <summary>
    /// Lerps the health bar image fill amount from a start value to an end value
    /// </summary>
    /// <param name="healthBeforeDamage">the health value before damage</param>
    /// <param name="healthAfterDamage">the health value after damage</param>
    /// <returns></returns>
    private IEnumerator UpdateHealthBar(float healthBeforeDamage, float healthAfterDamage)
    {
        float elapsedTime = 0;
        float timer = 1f;

        // move vertically
        while (elapsedTime < timer)
        {
            float currentHp = Mathf.Lerp(healthBeforeDamage, healthAfterDamage, (elapsedTime / timer));
            HealthBar.fillAmount = currentHp / MaxHealthStat;
            BattleUnitStats[StatName.Health] = Mathf.Round(currentHp);
            UpdateStats();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        HealthBar.fillAmount = healthAfterDamage / MaxHealthStat;
        yield return null;
    }

    /// <summary>
    /// Updates the battle stats of a BattleUnit.
    /// </summary>
    /// <param name="currentHealth">the new health value</param>
    /// <param name="battleUnit">reference to the BattleUnit type</param>
    public virtual void UpdateStats()
    {
        string levelNameString = $"LVL {BattleUnitStats[StatName.Level]} {BattleUnitInfo.BattleUnitName}";
        string healthString = $"\nHP : {BattleUnitStats[StatName.Health]} / {MaxHealthStat}";
        string attackString = $"\nATK : {BattleUnitStats[StatName.Attack]}";
        string defenseString = $"\nDEF : {BattleUnitStats[StatName.Defense]}";
        string speedString = $"\nSPD : {BattleUnitStats[StatName.Speed]}";
        string movementString = $"\nMOV : {BattleUnitStats[StatName.Movement]}";

        string battleStatsString =
            levelNameString +
            healthString +
            attackString +
            defenseString +
            speedString +
            movementString;


        UnitBattleStatsText.SetText(battleStatsString);
    }

    /// <summary>
    /// Initializes AllTilePositionsInMovementRange with all tiles reachable from current position.
    /// Initializes AllTilePositionsInAttackRange with all tiles attackable from current position.
    /// </summary>
    /// <param name="startPosition">the current position</param>
    public void UpdateAttackAndMovementRange(Vector3 startPosition)
    {
        startPosition = new Vector3(startPosition.x, startPosition.y, 0);
        AllTilePositionsInMovementRange = initializeValidPositions(transform.position);
        calculateValidMovementPositions(1, AllTilePositionsInMovementRange);
        AllTilePositionsInAttackRange = calculateTilesInAttackRange(AllTilePositionsInMovementRange);
        UpdateAttackAndMovementRangeIndicator();
    }

    /// <summary>
    /// Creates a set of the startingPosition unioned with its four adjacent tiles, if they are not occupied/// 
    /// </summary>
    /// <param name="startingPosition">the starting position</param>
    /// <returns>the positions of unoccupied tiles adjacent to the starting position</returns>
    private HashSet<Vector3> initializeValidPositions(Vector3 startingPosition)
    {
        HashSet<Vector3> res = new HashSet<Vector3>();
        res.Add(startingPosition);

        if (BattleUnitStats[StatName.Movement] == 0) return res;

        Vector3 pos1 = startingPosition + Vector3.up;
        Vector3 pos2 = startingPosition + Vector3.down;
        Vector3 pos3 = startingPosition + Vector3.left;
        Vector3 pos4 = startingPosition + Vector3.right;


        if (CheckTilePositionContainsAllyUnit(pos1)) res.Add(pos1);
        if (CheckTilePositionContainsAllyUnit(pos2)) res.Add(pos2);
        if (CheckTilePositionContainsAllyUnit(pos3)) res.Add(pos3);
        if (CheckTilePositionContainsAllyUnit(pos4)) res.Add(pos4);

        return res;
    }
    
    /// <summary>
    /// Recursively calculates the movement range of a BattleUnit based on their movement stat 
    /// </summary>
    /// <param name="counter">the BattleUnit's movement stat</param>
    /// <param name="validPositions">the current set of valid positions</param>
    private void calculateValidMovementPositions(int counter, HashSet<Vector3> validPositions)
    {
        HashSet<Vector3> newValidPositions = new HashSet<Vector3>();
        if (counter >= BattleUnitStats[StatName.Movement])
        {
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

                if (CheckTilePositionContainsAllyUnit(pos1)) newValidPositions.Add(pos1);
                if (CheckTilePositionContainsAllyUnit(pos2)) newValidPositions.Add(pos2);
                if (CheckTilePositionContainsAllyUnit(pos3)) newValidPositions.Add(pos3);
                if (CheckTilePositionContainsAllyUnit(pos4)) newValidPositions.Add(pos4);
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

    /// <summary>
    /// Finds the closest position to the attacking BattleUnit. 
    /// </summary>
    /// <param name="validAttackPositions">list of attack positions to consider</param>
    /// <returns>the closest attack position</returns>
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

    /// <summary>
    /// Checks if the current tile position is empty.
    /// </summary>
    /// <param name="position">the position to check</param>
    /// <returns>true if no battle unit is found</returns>
    private bool CheckNoBattleUnitOnTile(Vector3 position)
    {
        Collider2D col = Physics2D.OverlapPoint(position, Constants.MASK_BATTLE_UNIT);
        return col == null;
    }

    /// <summary>
    /// Checks if the current tile position is occupied by an ally unit. 
    /// </summary>
    /// <param name="position">the position to check</param>
    /// <returns>true if tile is contains an ally, false if tile contains an enemy unit</returns>
    private bool CheckTilePositionContainsAllyUnit(Vector3 position)
    {
        LayerMask sameObjectLayerMask = 1 << gameObject.layer;
        Collider2D objectNotInSameLayer = Physics2D.OverlapPoint(position, ~sameObjectLayerMask);
        Collider2D objectInSameLayer = Physics2D.OverlapPoint(position, sameObjectLayerMask);


        // if the collider is not an ally remove it from the reachable tiles set
        if (objectNotInSameLayer != null) return false;

        // this might be unecessary
        // if the collider IS an ally but {MovementStat} tiles away from current position, remove it from the reachable tiles set
        if (objectInSameLayer != null && isOnEdgeTileOfMovementRange(transform.position, position)) return false;

        return true;
    }

    /// <summary>
    /// Turns BattleUnit's sprite gray to indicate their turn is over.
    /// </summary>
    public void ChangeColorToIndicateBattleUnitTurnOver()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color;
        if (ColorUtility.TryParseHtmlString(Constants.ATTACK_FINISHED_COLOR, out color))
        {
            spriteRenderer.color = color;
        }
    }

    /// <summary>
    /// Restores BattleUnit's original sprite color.
    /// </summary>
    public void RestoreBattleUnitOriginalColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color;
        if (ColorUtility.TryParseHtmlString(Constants.ORIGINAL_COLOR, out color))
        {
            spriteRenderer.color = color;
        }
    }

    /// <summary>
    /// Checks if a tile is on the edge of a BattleUnit's movement range.
    /// </summary>
    /// <param name="startPosition">current position of the BattleUnit</param>
    /// <param name="endPosition">tile position to check</param>
    /// <returns>Whether or not the endPosition is BattleUnit.MovementStat tiles away</returns>
    private bool isOnEdgeTileOfMovementRange(Vector3 startPosition, Vector3 endPosition)
    {
        float startX = startPosition.x; 
        float startY = startPosition.y; 
        float endX = endPosition.x; 
        float endY = endPosition.y;
        return BattleUnitStats[StatName.Movement] == Mathf.Abs(endX - startX) + Mathf.Abs(endY - startY);
    }

    public abstract IEnumerator HandleBattleUnitDeath(BattleUnit attackingUnit);
}
