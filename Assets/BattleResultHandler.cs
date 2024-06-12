using UnityEngine;
using TMPro;

public class BattleResultHandler
{
    private GameObject battleResultHolder;
    private GameObject attackingUnit;
    private GameObject battleResult;
    private GameObject defendingUnit;

    private TMP_Text attackingUnitText; 
    private TMP_Text battleResultText; 
    private TMP_Text unitAttackCount; 
    private TMP_Text defendingUnitText;

    private BattleUnit currentAttackingUnit;
    private BattleUnit currentDefendingUnit;

    public BattleResultHandler(GameObject battleResultHolder, GameObject attackingUnit, GameObject battleResult, TMP_Text unitAttackCount, GameObject defendingUnit)
    {
        this.battleResultHolder = battleResultHolder;
        this.attackingUnit = attackingUnit;
        this.battleResult= battleResult;
        this.defendingUnit = defendingUnit;

        attackingUnitText =  attackingUnit.GetComponentInChildren<TMP_Text>();
        battleResultText =  battleResult.GetComponentInChildren<TMP_Text>();
        this.unitAttackCount = unitAttackCount;
        defendingUnitText =  defendingUnit.GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// Makes the battle result UI visible.
    /// </summary>
    public void TurnOnBattleResultCanvas()
    {
        UpdateBattleResultCanvas();
        battleResultHolder.SetActive(true);
    }

    /// <summary>
    /// Makes the battle result UI invisible.
    /// </summary>
    public void TurnOffBattleResultCanvas()
    {
        battleResultHolder.SetActive(false);
    }

    /// <summary>
    /// Sets the text to information about the attacking unit.
    /// </summary>
    public void SetAttackingUnitText()
    {
        attackingUnitText.SetText(currentAttackingUnit.BattleUnitInfo.BattleUnitName);
    }

    /// <summary>
    /// Sets the text to information about the battle result.
    /// </summary>
    public void SetBattleResultText()
    {
        battleResultText.SetText($"{DetermineDamageToTarget()}");
    }

    /// <summary>
    /// Sets the text to the number of times the attacking unit will attack.
    /// </summary>
    public void SetUnitAttackCountText()
    {
        float attackCount = DetermineNumberOfAttacks();
        if (attackCount > 1) {
            unitAttackCount.SetText($"X {attackCount}");
        }
        else
        {
            unitAttackCount.SetText($"");
        }
    }

    /// <summary>
    /// Setse the text to information about the defending unit.
    /// </summary>
    public void SetDefendingUnitText()
    {
        defendingUnitText.SetText(currentDefendingUnit.BattleUnitInfo.BattleUnitName);
    }

    /// <summary>
    /// Used to assign the current attacking battle unit.
    /// </summary>
    /// <param name="currentAttackingUnit">the battle unit attacking</param>
    public void SetCurrentAttackingUnit(BattleUnit currentAttackingUnit)
    {
        this.currentAttackingUnit = currentAttackingUnit;
    }

    /// <summary>
    /// Used to assign the current defending battle unit.
    /// </summary>
    /// <param name="currentDefendingUnit">the battle unit defending</param>
    public void SetCurrentDefendingUnit(BattleUnit currentDefendingUnit)
    {
        this.currentDefendingUnit = currentDefendingUnit;
    }

    /// <summary>
    /// Updates the text of the battle result screen. 
    /// </summary>
    public void UpdateBattleResultCanvas()
    {
        SetAttackingUnitText();
        SetBattleResultText();
        SetUnitAttackCountText();
        SetDefendingUnitText();
    }

    /// <summary>
    /// Calculates the number of times the attacking unit will attack.
    /// </summary>
    /// <returns>the number of times to attack</returns>
    public int DetermineNumberOfAttacks()
    {
        float attackingUnitSpeed = currentAttackingUnit.BattleUnitStats[StatName.Speed];
        float defendingUnitSpeed = currentDefendingUnit.BattleUnitStats[StatName.Speed];
        if (attackingUnitSpeed > defendingUnitSpeed * 2) return 2;

        return 1;
    }

    /// <summary>
    /// Calculates the amount of damage the attacking unit will deal to the defending unit.
    /// </summary>
    /// <returns>the amount of damage to deal</returns>
    public float DetermineDamageToTarget()
    {
        float attackingUnitAttack = currentAttackingUnit.BattleUnitStats[StatName.Attack];
        float defendingUnitDefense = currentDefendingUnit.BattleUnitStats[StatName.Defense];

        return attackingUnitAttack - defendingUnitDefense;
    }
}
