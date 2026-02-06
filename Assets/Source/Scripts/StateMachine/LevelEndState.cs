using Cysharp.Threading.Tasks;

public class LevelEndState : IState<GameState>
{
    public override GameState Key => GameState.LevelEndState;

    public LevelEndState(IStateSwitcher<GameState> stateSwitcher) : base(stateSwitcher)
    {
    }

    public override async UniTask Enter()
    {
        //cleanup units

        await _stateSwitcher.SwitchState(GameState.MenuState);
    }

    public override async UniTask Exit()
    {
        await UniTask.CompletedTask;
    }
}
