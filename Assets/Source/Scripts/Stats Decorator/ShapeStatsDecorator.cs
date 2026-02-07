public class ShapeStatsDecorator : StatsDecorator
{
    private readonly ShapeStatsModifier _mod;

    public ShapeStatsDecorator(IStats stats, ShapeStatsModifier mod) : base(stats)
    {
        _mod = mod;
    }

    public override float HP => _stats.HP + _mod.HP;
    public override float ATK => _stats.ATK + _mod.ATK;
}

