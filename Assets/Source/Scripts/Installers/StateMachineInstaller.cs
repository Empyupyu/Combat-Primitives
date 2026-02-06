using UnityEngine;
using Zenject;
public class StateMachineInstaller : MonoInstaller
{
    [SerializeField] private GameStateMachineMono _stateMachine;

    public override void InstallBindings()
    {
        StateMachineMonoInstall();
        GameStateInstall();
        GameStateMachineInstall();

        Container.BindInterfacesAndSelfTo<GameStartInitializer>().AsSingle();
    }

    private void StateMachineMonoInstall()
    {
        Container.Bind<GameStateMachineMono>()
                   .FromInstance(_stateMachine)
                   .AsSingle();

        Container.Bind<IStateSwitcher<GameState>>().
            FromInstance(_stateMachine).
            AsSingle();
    }

    private void GameStateInstall()
    {
        Container.Bind<IState<GameState>>().To<BootState>().AsSingle();
        Container.Bind<IState<GameState>>().To<MenuState>().AsSingle();
        Container.Bind<IState<GameState>>().To<GameLoopState>().AsSingle();
        Container.Bind<IState<GameState>>().To<LevelEndState>().AsSingle();
    }

    private void GameStateMachineInstall()
    {
        Container.Bind<StateMachine<GameState>>().AsSingle();

        _stateMachine.SetStateMachine(Container.Resolve<StateMachine<GameState>>());
    }
}
