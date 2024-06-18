using System.Collections.Generic;

public abstract class Skill
{
    public  abstract string SkillName { get; set; }
    public abstract string SkillDescription { get; set; }
    public abstract bool EquipableSkill { get; }
    public abstract void ApplySkillEffect(List<PlayerUnit> playerUnits);
}
