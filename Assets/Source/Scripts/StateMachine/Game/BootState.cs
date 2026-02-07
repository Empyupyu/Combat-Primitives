using Cysharp.Threading.Tasks;

public class BootState : IState<GameState>
{
    public override GameState Key => GameState.BootState;

    private readonly LevelLoader _levelLoader;

    public BootState(IStateSwitcher<GameState> stateSwitcher,
        LevelLoader levelLoader) : base(stateSwitcher)
    {
        _levelLoader = levelLoader;
    }

    public override async UniTask Enter()
    {
        await _levelLoader.LoadLevel();

        await _stateSwitcher.SwitchState(GameState.MenuState);
    }

    public override async UniTask Exit()
    {
        await UniTask.CompletedTask;
    }
}
