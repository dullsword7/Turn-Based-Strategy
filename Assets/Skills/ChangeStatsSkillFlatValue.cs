using System.Collections.Generic;

public class ChangeStatsSkillFlatValue : PassiveSkill
{
    private string skillName;
    private string skillDescription;
    private float statIncrease;
    private StatName statName;
    public override string SkillName { get => skillName; set => skillName = value; }
    public override string SkillDescription { get => skillDescription; set => skillDescription = value; }
    public override bool EquipableSkill { get => false; }

    public ChangeStatsSkillFlatValue(float statIncrease, StatName statName)
    {
        this.statIncrease = statIncrease;
        this.statName = statName;

        skillName = $"Insane Omega {statName} Boost";
        skillDescription = $"Greatly Increases Your {statName}";
    }

    public override void ApplySkillEffect(List<PlayerUnit> playerUnits)
    {
        foreach (PlayerUnit playerUnit in playerUnits)
        {
            playerUnit.BattleUnitStats[statName] += statIncrease;
            playerUnit.UpdateStats();
        }
    }
}
