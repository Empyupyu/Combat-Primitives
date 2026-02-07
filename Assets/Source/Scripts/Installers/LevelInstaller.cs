using Zenject;

public class LevelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<UnitSpawnStrategy>().To<CircleSpawnStrategy>().AsSingle().WithArguments(1.2f);
        Container.Bind<IStatsCreatorStrategy>().To<RandomStatsCreatorStrategy>().AsSingle();
        Container.Bind<StatsFactory>().AsSingle();
        Container.Bind<StatsCreator>().AsSingle();

        Container.Bind<UnitSpawner>()
            .AsSingle()
            .OnInstantiated<UnitSpawner>((ctx, spawner) =>
            {
                var controller = ctx.Container.Resolve<UnitSpawnController>();
                controller.RegisterSpawner(spawner);
            });

        Container.Resolve<UnitSpawner>();
    }
}
