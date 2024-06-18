using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantBeDoubledSkill : Skill
{
    private string skillName;
    private string skillDescription;
    public override string SkillName { get => skillName; set => skillName = value; }
    public override string SkillDescription { get => skillDescription; set => skillDescription = value; }

    public override bool EquipableSkill => throw new System.NotImplementedException();

    public CantBeDoubledSkill()
    {
        skillName = $"Soooonic";
        skillDescription = $"Unit Cannot Be Double In Combat.";
    }

    public override void ApplySkillEffect(List<PlayerUnit> playerUnits)
    {
        foreach (PlayerUnit playerUnit in playerUnits)
        {
            playerUnit.BattleUnitStats[StatName.Speed] = float.MaxValue;
        }
    }
}
