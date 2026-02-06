using UnityEngine;
using Zenject;

public class LevelConfigsInstaller : MonoInstaller
{
    [SerializeField] private LevelConfig _levelConfig;

    public override void InstallBindings()
    {
        Container.BindInstance(_levelConfig).AsSingle();
    }
}
