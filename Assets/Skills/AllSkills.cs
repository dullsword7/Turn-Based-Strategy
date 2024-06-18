using System.Collections.Generic;

public class AllSkills 
{
    public List<Skill> PossibleSkills;

    private ChangeStatsSkillFlatValue increaseHealthSkill;
    private ChangeStatsSkillFlatValue increaseAttackSkill;
    private ChangeStatsSkillFlatValue increaseDefenseSkill;
    private ChangeStatsSkillFlatValue increaseSpeedSkill;

    public AllSkills()
    {
        PossibleSkills = new List<Skill>();
        increaseHealthSkill = new ChangeStatsSkillFlatValue(10, StatName.Health);
        increaseAttackSkill = new ChangeStatsSkillFlatValue(10, StatName.Attack);
        increaseDefenseSkill = new ChangeStatsSkillFlatValue(10, StatName.Defense);
        increaseSpeedSkill = new ChangeStatsSkillFlatValue(10, StatName.Speed);

        InitializeSkillList();
    }

    private void InitializeSkillList()
    {
        PossibleSkills.Add(increaseHealthSkill);
        PossibleSkills.Add(increaseAttackSkill);
        PossibleSkills.Add(increaseDefenseSkill);
        PossibleSkills.Add(increaseSpeedSkill);
    }
}
