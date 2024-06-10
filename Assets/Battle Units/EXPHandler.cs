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

        while (elapsedTime < timer)
        {
            float currentExp = Mathf.Lerp(expBefore, expAfter, elapsedTime / timer);

            if ( currentExp >= playerUnit.ExpToNextLevel) 
            {
                expBefore = 0;
                expAfter = expValue - playerUnit.ExpToNextLevel;
                expValue -= playerUnit.ExpToNextLevel;
                playerUnit.CurrentExp = 0;
                playerUnit.CurrentLevel += 1;
                playerUnit.UpdateStats();
                AdjustExpRequiredForNextLevel();
                timer -= elapsedTime;
                elapsedTime = 0;


                yield return new WaitForSeconds(1);
            }

            playerUnit.ExpBar.fillAmount =  currentExp / playerUnit.ExpToNextLevel;
            playerUnit.CurrentExp = Mathf.Round(currentExp);
            playerUnit.UpdateStats();

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerUnit.ExpBar.fillAmount = expAfter / playerUnit.ExpToNextLevel;

        Debug.Log($"Final Exp is {playerUnit.CurrentExp} / {playerUnit.ExpToNextLevel}");

        yield return new WaitForSeconds(1f);
        playerUnit.TurnOffInfo();
    }

    private void AdjustExpRequiredForNextLevel()
    {
        playerUnit.ExpToNextLevel = Mathf.Round(playerUnit.ExpToNextLevel * 1.5f);
        Debug.Log(playerUnit.ExpToNextLevel);
    }
}
