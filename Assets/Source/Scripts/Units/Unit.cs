using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

public class UnitView : MonoBehaviour
{
    [field: SerializeField] public GameObject Model {  get; private set; }
    [field: SerializeField] public Unit Unit {  get; private set; }
    [field: SerializeField] public NavMeshAgent Agent {  get; private set; }
    [field: SerializeField] public UnitHealthView UnitHealthView {  get; private set; }

    public void SetUnit(Unit unit)
    {
        Unit = unit;
    }
}

public class Unit : IDamageble
{
    public event Action<float> OnHealthChange;
    public event Action<Unit> OnDeath;
    public UnitTeam UnitTeam { get; private set; }

    private IStats _baseStats;

    private float _currentHealth;

    public Unit(UnitTeam unitTeam, IStats baseStats)
    {
        UnitTeam = unitTeam;
        _baseStats = baseStats;
        _currentHealth = _baseStats.HP;
    }

    public void ApplyDamage(float value)
    {
        _currentHealth -= value;

        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            OnDeath?.Invoke(this);
        }

        OnHealthChange?.Invoke(value);
    }
}

public class UnitHealthView : MonoBehaviour
{
    [field: SerializeField] public Image HealthFill { get; private set; }
}

public interface IDamageble
{
    public void ApplyDamage(float value);
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

public class StatsFactory
{
    private readonly UnitModifierConfig _config;

    public StatsFactory(UnitModifierConfig modifiers)
    {
        _config = modifiers;
    }

    public IStats Create(ShapeType shape, SizeType size, ColorType color)
    {
        IStats stats = new BaseStats(_config.UnitBaseConfig);

        var shapeMod = _config.Shapes.First(x => x.ShapeType == shape).ShapeStatsModifier;
        stats = new ShapeStatsDecorator(stats, shapeMod);

        var sizeMod = _config.Sizes.First(x => x.SizeType == size).SizeStatsModifier;
        stats = new SizeStatsDecorator(stats, sizeMod);

        var colorMod = _config.Colors.First(x => x.ColorType == color).ColorStatsModifier;
        stats = new ColorStatsDecorator(stats, colorMod);

        return stats;
    }
}


public interface IStats
{
    public float HP { get; }
    public float ATK { get; }
    public float SPEED { get; }
    public float ATKSPD { get; }
}

public class UnitSpawner
{
    private readonly UnitSpawnStrategy _unitSpawnStrategy;
    private readonly UnitView _unitPrefab;
    private readonly StatsFactory _statsFactory;
    private readonly StatsCreator _statsCreator;
    private readonly UnitModifierConfig _unitModifierConfig;
    private Dictionary<UnitTeam, List<UnitView>> _units;

    public UnitSpawner(
        UnitSpawnStrategy unitSpawnStrategy,
        UnitView unitPrefab,
        StatsFactory statsFactory,
        StatsCreator statsCreator,
        UnitModifierConfig unitModifierConfig)
    {
        _unitSpawnStrategy = unitSpawnStrategy;
        _unitPrefab = unitPrefab;
        _statsFactory = statsFactory;
        _statsCreator = statsCreator;
        _unitModifierConfig = unitModifierConfig;

        _units = new Dictionary<UnitTeam, List<UnitView>>();
    }

    public void Spawn(UnitTeam unitTeam, Vector3 startPoint, Vector3 otherTeamPoint)
    {
        UnitView unitView = CreateUnitView(startPoint, otherTeamPoint);

        if (!_units.ContainsKey(unitTeam)) 
        {
            _units[unitTeam] = new List<UnitView>(); 
        }

        _units[unitTeam].Add(unitView);

        InitializeUnit(unitTeam, unitView);
    }

    private UnitView CreateUnitView(Vector3 startPoint, Vector3 otherTeamPoint)
    {
        Vector3 spawnPoint = _unitSpawnStrategy.CalculateSpawnPosition(startPoint);
        Vector3 lookPoint = new Vector3(spawnPoint.x, otherTeamPoint.y, otherTeamPoint.z);

        UnitView unitView = GameObject.Instantiate(_unitPrefab);
        unitView.transform.position = spawnPoint;
        unitView.transform.LookAt(lookPoint);
        return unitView;
    }

    private void InitializeUnit(UnitTeam unitTeam, UnitView unitView)
    {
        UnitStatsSetup unitStatsSetup = _statsCreator.GetStatSetup();
        GameObject.Instantiate(_unitModifierConfig.Shapes.
          First(x => x.ShapeType == unitStatsSetup.ShapeType).ShapeMesh, unitView.transform);

        IStats stats = _statsFactory.Create(unitStatsSetup.ShapeType, unitStatsSetup.SizeType, unitStatsSetup.ColorType);

        Unit unit = new Unit(unitTeam,stats);
        unitView.SetUnit(unit);
    }
}

public class StatsCreator
{
    private readonly IStatsCreatorStrategy _strategy;

    public StatsCreator(IStatsCreatorStrategy strategy)
    {
        _strategy = strategy;
    }

    public UnitStatsSetup GetStatSetup()
    {
        return _strategy.GetStatsSetup();
    }
}

public interface IStatsCreatorStrategy
{
    public UnitStatsSetup GetStatsSetup();
}

public class RandomStatsCreatorStrategy : IStatsCreatorStrategy
{
    public UnitStatsSetup GetStatsSetup()
    {
        return new UnitStatsSetup(EnumExtensions.GetRandomValue<ShapeType>(),
            EnumExtensions.GetRandomValue<SizeType>(),
            EnumExtensions.GetRandomValue<ColorType>());
    }
}

public class UnitStatsSetup
{
    public ShapeType ShapeType { get; }
    public SizeType SizeType { get; }
    public ColorType ColorType { get; }

    public UnitStatsSetup(ShapeType shapeType, SizeType sizeType, ColorType colorType)
    {
        ShapeType = shapeType;
        SizeType = sizeType;
        ColorType = colorType;
    }
}

public class UnitBaseStatsConfig : ScriptableObject
{
    [field: SerializeField] public float HP { get; private set; }
    [field: SerializeField] public float ATK { get; private set; }
    [field: SerializeField] public float SPEED { get; private set; }
    [field: SerializeField] public float ATKSPD { get; private set; }
}

[CreateAssetMenu(menuName = "Configs/UnitStatsModifiers")]
public class UnitModifierConfig : ScriptableObject
{
    [field: SerializeField] public UnitBaseStatsConfig UnitBaseConfig { get; private set; }
    [field: SerializeField] public List<UnitShape> Shapes { get; private set; }
    [field: SerializeField] public List<UnitSize> Sizes { get; private set; }
    [field: SerializeField] public List<UnitColor> Colors { get; private set; }
}

[Serializable]
public class UnitShape
{
    public ShapeType ShapeType;
    public GameObject ShapeMesh;
    public ShapeStatsModifier ShapeStatsModifier;
}

public enum ShapeType
{
    Cube,
    Sphere
}

public enum ColorType
{
    Blue,
    Green,
    Red
}

[Serializable]
public class UnitColor
{
    public ColorType ColorType;
    public ColorStatsModifier ColorStatsModifier;
}

public class ColorStatsModifier
{
    public float HP;
    public float ATK;
    public float SPEED;
    public float ATKSPD;
}

public class ShapeStatsModifier
{
    public float HP;
    public float ATK;
}

public class SizeStatsModifier
{
    public float HP;
}

[Serializable]
public class UnitSize
{
    public SizeType SizeType;
    public SizeStatsModifier SizeStatsModifier;
}

public enum SizeType
{
    Big,
    Small
}

public enum UnitTeam
{
    Team1,
    Team2
}

public interface UnitSpawnStrategy
{
    public Vector3 CalculateSpawnPosition(Vector3 startPoint);
}

public class CircleSpawnStrategy : UnitSpawnStrategy
{
    private readonly float _radius;

    public CircleSpawnStrategy(float radius)
    {
        _radius = radius;
    }

    public Vector3 CalculateSpawnPosition(Vector3 startPoint)
    {
        Vector2 circle = UnityEngine.Random.insideUnitSphere * _radius;
        return new Vector3(startPoint.x + circle.x, startPoint.y, startPoint.z + circle.y);
    }
}

