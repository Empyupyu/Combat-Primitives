using Cysharp.Threading.Tasks;

public class GameLoopState : IState<GameState>
{
    private readonly DamageService _damageService;

    public override GameState Key => GameState.GameLoopState;

    public GameLoopState(IStateSwitcher<GameState> stateSwitcher,
        DamageService damageService) : base(stateSwitcher)
    {
        _damageService = damageService;
    }

    public override async UniTask Enter()
    {
        _damageService.AllUnitsInTeamDeath += LevelEnd;
        await UniTask.CompletedTask;
    }

    private void LevelEnd()
    {
        _damageService.AllUnitsInTeamDeath -= LevelEnd;
        _stateSwitcher.SwitchState(GameState.LevelEndState);
    }

    public override async UniTask Exit()
    {
        await UniTask.CompletedTask;
    }
}
