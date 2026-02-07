public abstract class StatsDecorator : IStats
{
    protected readonly IStats _stats;

    protected StatsDecorator(IStats stats) 
    {
        _stats = stats;
    }

    public virtual float HP => _stats.HP;
    public virtual float ATK => _stats.ATK;
    public virtual float SPEED => _stats.SPEED;
    public virtual float ATKSPD => _stats.ATKSPD;
}

