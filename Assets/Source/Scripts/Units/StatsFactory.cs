using System.Linq;

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

