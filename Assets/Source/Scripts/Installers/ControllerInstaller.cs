using Zenject;

public class ControllerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MenuController>().AsSingle();
        Container.Bind<UnitSpawnController>().AsSingle();
        Container.Bind<DamageService>().AsSingle();
        Container.Bind<BattleController>().AsSingle();
        Container.Bind<UnitsController>().AsSingle();
    }
}
