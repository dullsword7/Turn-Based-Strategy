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

            if (currentExp >= playerUnit.ExpToNextLevel) 
            {
                expBefore = 0;
                expAfter = expValue - playerUnit.ExpToNextLevel;
                expValue -= playerUnit.ExpToNextLevel;
                playerUnit.CurrentExp = 0;
                playerUnit.BattleUnitStats[StatName.Level] += 1;
                AdjustPlayerStatsOnLevelUp();
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

        yield return new WaitForSeconds(1f);
        playerUnit.TurnOffInfo();
    }

    /// <summary>
    /// Adjusts the experience required to reach the next level when a player unit levels up.
    /// </summary>
    private void AdjustExpRequiredForNextLevel()
    {
        playerUnit.ExpToNextLevel = Mathf.Round(playerUnit.ExpToNextLevel * 1.5f);
    }

    /// <summary>
    /// Generates a number to check whether or not each stat should be increased based on the stat's growth rate.
    /// </summary>
    private void AdjustPlayerStatsOnLevelUp()
    {
        float statCheck = Random.Range(1, 100);

        if (statCheck <= playerUnit.BattleUnitStatsGrowthRates[StatName.Health])
        {
            playerUnit.MaxHealthStat = Mathf.Round(playerUnit.MaxHealthStat * 1.1f);
            playerUnit.BattleUnitStats[StatName.Health] = Mathf.Round(playerUnit.BattleUnitStats[StatName.Health] * 1.1f);
        }
        if (statCheck <= playerUnit.BattleUnitStatsGrowthRates[StatName.Attack])
            playerUnit.BattleUnitStats[StatName.Attack] = Mathf.Round(playerUnit.BattleUnitStats[StatName.Attack] * 1.1f);
    }
}
