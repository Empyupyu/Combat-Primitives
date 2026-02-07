public class RandomStatsCreatorStrategy : IStatsCreatorStrategy
{
    public UnitStatsSetup GetStatsSetup()
    {
        return new UnitStatsSetup(EnumExtensions.GetRandomValue<ShapeType>(),
            EnumExtensions.GetRandomValue<SizeType>(),
            EnumExtensions.GetRandomValue<ColorType>());
    }
}

