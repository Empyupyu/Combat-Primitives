using UnityEngine;
using Zenject;

public class LevelViewInstaller : MonoInstaller
{
    [SerializeField] private UnitView _unitView;

    public override void InstallBindings()
    {
        Container.Bind<UnitView>().FromInstance(_unitView).AsSingle();
    }
}