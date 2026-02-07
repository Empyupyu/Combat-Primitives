public class ColorStatsDecorator : StatsDecorator
{
    private readonly ColorStatsModifier _mod;

    public ColorStatsDecorator(IStats stats, ColorStatsModifier mod) : base(stats)
    {
        _mod = mod;
    }

    public override float HP => _stats.HP + _mod.HP;
    public override float ATK => _stats.ATK + _mod.ATK;
    public override float SPEED => _stats.SPEED + _mod.SPEED;
    public override float ATKSPD => _stats.ATKSPD + _mod.ATKSPD;
}

