using UnityEngine;
using UnityEngine.UI;

public class Unit
{
    public UnitTeam UnitTeam { get; private set; }
    public float CurrentHealth { get; private set; }

    private IStats _baseStats;

    public Unit(UnitTeam unitTeam, IStats baseStats)
    {
        UnitTeam = unitTeam;
        _baseStats = baseStats;
        CurrentHealth = _baseStats.HP;
    }

    public void ApplyDamage(float value)
    {
        CurrentHealth -= value;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
        }
    }
}

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

public class SizeStatsDecorator : StatsDecorator
{
    private readonly SizeStatsModifier _mod;

    public SizeStatsDecorator(IStats stats, SizeStatsModifier mod) : base(stats)
    {
        _mod = mod;
    }

    public override float HP => _stats.HP + _mod.HP;
}

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


public interface IStats
{
    public float HP { get; }
    public float ATK { get; }
    public float SPEED { get; }
    public float ATKSPD { get; }
}

