using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EXPHandler 
{
    PlayerUnit playerUnit;
    public EXPHandler(PlayerUnit playerUnit)
    {
        this.playerUnit = playerUnit;
    }

    /// <summary>
    /// Increases the BattleUnit's exp and updates their exp bar.
    /// </summary>
    /// <param name="playerUnit">the playerUnit receiving the exp</param>
    /// <param name="expValue">the amount of exp to recieve</param>
    public IEnumerator UpdateExp(PlayerUnit playerUnit, float expValue)
    {
        playerUnit.TurnOnInfo();

        float elapsedTime = 0;
        float timer = 1f;

        float expBefore = playerUnit.CurrentExp;
        float expAfter = expBefore + expValue;

        Debug.Log($"Lerping from {expBefore} to {expAfter}");

        while (elapsedTime < timer)
        {
            float currentExp = Mathf.Lerp(expBefore, expAfter, (elapsedTime / timer));
            playerUnit.ExpBar.fillAmount =  currentExp / playerUnit.ExpToNextLevel;
            playerUnit.CurrentExp = Mathf.Round(currentExp);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerUnit.ExpBar.fillAmount = expAfter / playerUnit.ExpToNextLevel;

        yield return new WaitForSeconds(1f);
        playerUnit.TurnOffInfo();
    }
}
