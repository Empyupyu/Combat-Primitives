public class BaseStats : IStats
{
    public float HP { get; }
    public float ATK { get; }
    public float SPEED { get; }
    public float ATKSPD { get; }

    public BaseStats(UnitBaseStatsConfig unitBaseStatsConfig)
    {
        HP = unitBaseStatsConfig.HP;
        ATK = unitBaseStatsConfig.ATK;
        SPEED = unitBaseStatsConfig.SPEED;
        ATKSPD = unitBaseStatsConfig.ATKSPD;
    }
}

