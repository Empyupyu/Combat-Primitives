using Zenject;

public class AssetLoaderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IAssetLoader>().To<AddressableAssetLoader>().AsSingle();
        Container.Bind<LevelLoader>().AsSingle();
    }
}