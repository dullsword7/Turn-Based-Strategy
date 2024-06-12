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

    public void SetAttackingUnitText()
    {
        attackingUnitText.SetText(currentAttackingUnit.BattleUnitInfo.BattleUnitName);
    }

    public void SetBattleResultText()
    {
        battleResultText.SetText($"{currentAttackingUnit.BattleUnitInfo.BattleUnitName} VS {currentDefendingUnit.BattleUnitInfo.BattleUnitName}");
    }

    public void SetDefendingUnitText()
    {
        defendingUnitText.SetText(currentDefendingUnit.BattleUnitInfo.BattleUnitName);
    }

    public void SetCurrentAttackingUnit(BattleUnit currentAttackingUnit)
    {
        this.currentAttackingUnit = currentAttackingUnit;
    }
    public void SetCurrentDefendingUnit(BattleUnit currentDefendingUnit)
    {
        this.currentDefendingUnit = currentDefendingUnit;
    }
    public void UpdateBattleResultCanvas()
    {
        SetAttackingUnitText();
        SetBattleResultText();
        SetDefendingUnitText();
    }
}
