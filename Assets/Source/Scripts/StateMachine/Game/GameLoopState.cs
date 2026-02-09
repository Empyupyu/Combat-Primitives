using Cysharp.Threading.Tasks;

public class GameLoopState : IState<GameState>
{
    public override GameState Key => GameState.GameLoopState;

    private readonly BattleController _battleController;
    private readonly UnitsController _unitsController;

    public GameLoopState(IStateSwitcher<GameState> stateSwitcher,
        BattleController battleController,
        UnitsController unitsController) : base(stateSwitcher)
    {
        _battleController = battleController;
        _unitsController = unitsController;
    }

    public override async UniTask Enter()
    {
        _battleController.OnBattleFinished += LevelEnd;

        _unitsController.SetAllUnitsState(UnitState.Chase);

        await UniTask.CompletedTask;
    }

    private void LevelEnd()
    {
        _battleController.OnBattleFinished -= LevelEnd;
        _stateSwitcher.SwitchState(GameState.LevelEndState);
    }

    public override async UniTask Exit()
    {
        await UniTask.Delay(Constants.DelayBeforeEndGameLoopState);
    }
}
