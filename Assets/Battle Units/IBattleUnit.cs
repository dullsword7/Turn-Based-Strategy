public interface IBattleUnit 
{
    public BattleStatsScriptableObject UnitBattleStats { get; set; }
    public void InitalizeBattleStats();
    public void DealDamage(float damageAmount);

}
