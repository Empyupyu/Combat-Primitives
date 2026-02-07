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

        var shapeMod = _config.GetShape(shape);
        stats = new ShapeStatsDecorator(stats, shapeMod);

        var sizeMod = _config.GetSize(size);
        stats = new SizeStatsDecorator(stats, sizeMod);

        var colorMod = _config.GetColor(color);
        stats = new ColorStatsDecorator(stats, colorMod);

        return stats;
    }
}

