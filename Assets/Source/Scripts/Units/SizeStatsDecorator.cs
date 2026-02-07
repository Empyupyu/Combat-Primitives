public class SizeStatsDecorator : StatsDecorator
{
    private readonly SizeStatsModifier _mod;

    public SizeStatsDecorator(IStats stats, SizeStatsModifier mod) : base(stats)
    {
        _mod = mod;
    }

    public override float HP => _stats.HP + _mod.HP;
}

