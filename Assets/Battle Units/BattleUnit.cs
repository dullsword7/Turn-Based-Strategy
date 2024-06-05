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

    public Action PlayerUnitDeath;
    public abstract GameObject UnitBattleStatsHolder { get; set; }
    public abstract GameObject HealthBarHolder { get; set; }
    public abstract HashSet<Vector3> ValidPositions { get; set; }
    public abstract Image HealthBar { get; set; }
    public abstract float MaxHealthStat { get; set; }
    public abstract float HealthStat { get; set; }
    public abstract float AttackStat { get; set; }
    public abstract float MovementStat { get; set; }
    public abstract TMP_Text UnitBattleStatsText { get; set; }
    public abstract void InitalizeBattleStats();

    // TODO ideally this will return the position closest to the attacking unit
    public virtual Vector3 ValidAttackPositions(Vector3 attackTargetPosition)
    {
        List<Vector3> attackPositions = new List<Vector3>();
        Vector3 pos1 = attackTargetPosition + Vector3.up;
        Vector3 pos2 = attackTargetPosition + Vector3.down;
        Vector3 pos3 = attackTargetPosition + Vector3.left;
        Vector3 pos4 = attackTargetPosition + Vector3.right;

        // if the attacker is already adjacent to its target, dont move
        if (transform.position == pos1 || transform.position == pos2 || transform.position == pos3 || transform.position == pos4) return transform.position;

        if (ValidPositions.Contains(pos1)) attackPositions.Add(pos1);
        if (ValidPositions.Contains(pos2)) attackPositions.Add(pos2);
        if (ValidPositions.Contains(pos3)) attackPositions.Add(pos3);
        if (ValidPositions.Contains(pos4)) attackPositions.Add(pos4);

        if (attackPositions.Count == 0) return attackTargetPosition;

        return attackPositions[0];
    }
    public virtual IEnumerator MoveToPosition(Vector3 attackTargetPosition, Action onComplete = null)
    {
        Vector3 targetDestination = ValidAttackPositions(attackTargetPosition);
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
}
