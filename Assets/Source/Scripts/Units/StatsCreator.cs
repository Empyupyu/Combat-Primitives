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

