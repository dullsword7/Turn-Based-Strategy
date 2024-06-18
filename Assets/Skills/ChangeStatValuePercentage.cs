using System.Collections.Generic;

public class ChangeStatValuePercentage : PassiveSkill
{
    private string skillName;
    private string skillDescription;
    private float statPercentIncrease;
    private StatName statName;
    public override string SkillName { get => skillName; set => skillName = value; }
    public override string SkillDescription { get => skillDescription; set => skillDescription = value; }
    public override bool EquipableSkill { get => false; }


    public ChangeStatValuePercentage(float statPercentIncrease, StatName statName)
    {
        this.statPercentIncrease = statPercentIncrease;
        this.statName = statName;

        skillName = $"Insane Omega {statName} Percentage Boost";
        skillDescription = $"Greatly Increases Your {statName} By {statPercentIncrease}%";
    }

    public override void ApplySkillEffect(List<PlayerUnit> playerUnits)
    {
        foreach (PlayerUnit playerUnit in playerUnits)
        {
            playerUnit.BattleUnitStats[statName] *= 1 + statPercentIncrease;
            playerUnit.UpdateStats();
        }
    }
}
