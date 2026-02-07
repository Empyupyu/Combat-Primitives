using UnityEngine;
using Zenject;

public class GameConfigsInstaller : MonoInstaller
{
    [SerializeField] private PrefabsKeyPath _prefabsKeyPath;
    [SerializeField] private UnitModifierConfig _unitModifierConfig;

    public override void InstallBindings()
    {
        _unitModifierConfig.Init();
        Container.BindInstance(_prefabsKeyPath).AsSingle();
        Container.BindInstance(_unitModifierConfig).AsSingle();
        Container.Bind<GameData>().AsSingle();
    }
}
