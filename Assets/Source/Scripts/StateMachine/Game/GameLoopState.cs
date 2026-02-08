using Cysharp.Threading.Tasks;

public class GameLoopState : IState<GameState>
{
    public override GameState Key => GameState.GameLoopState;

    private readonly BattleController _battleController;
    private readonly GameData _gameData;

    public GameLoopState(IStateSwitcher<GameState> stateSwitcher,
        BattleController battleController,
        GameData gameData) : base(stateSwitcher)
    {
        _battleController = battleController;
        _gameData = gameData;
    }

    public override async UniTask Enter()
    {
        _battleController.OnBattleFinished += LevelEnd;

        SetUnitsChaseState();

        await UniTask.CompletedTask;
    }

    private void SetUnitsChaseState()
    {
        foreach (var kvp in _gameData.CurrentUnitsInBattle)
        {
            foreach (var unitView in kvp.Value)
            {
                unitView.UnitStateMachineMono.SwitchState(UnitState.Chase).Forget();
            }
        }
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
