using System.Collections.Generic;

public class AllSkills 
{
    public List<Skill> PossibleSkills;

    private ChangeStatsSkill increaseHealthSkill;
    private ChangeStatsSkill increaseAttackSkill;
    private ChangeStatsSkill increaseDefenseSkill;
    private ChangeStatsSkill increaseSpeedSkill;

    public AllSkills()
    {
        PossibleSkills = new List<Skill>();
        increaseHealthSkill = new ChangeStatsSkill(10, StatName.Health);
        increaseAttackSkill = new ChangeStatsSkill(10, StatName.Attack);
        increaseDefenseSkill = new ChangeStatsSkill(10, StatName.Defense);
        increaseSpeedSkill = new ChangeStatsSkill(10, StatName.Speed);

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
