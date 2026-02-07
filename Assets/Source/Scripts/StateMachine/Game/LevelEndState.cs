using Cysharp.Threading.Tasks;

public class LevelEndState : IState<GameState>
{
    public override GameState Key => GameState.LevelEndState;

    private readonly UnitSpawnController _unitSpawnController;

    public LevelEndState(IStateSwitcher<GameState> stateSwitcher,
        UnitSpawnController unitSpawnController) : base(stateSwitcher)
    {
        _unitSpawnController = unitSpawnController;
    }

    public override async UniTask Enter()
    {
        _unitSpawnController.Cleanup();

        await _stateSwitcher.SwitchState(GameState.MenuState);
    }

    public override async UniTask Exit()
    {
        await UniTask.CompletedTask;
    }
}
