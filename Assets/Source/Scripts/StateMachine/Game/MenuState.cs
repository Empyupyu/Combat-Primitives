using Cysharp.Threading.Tasks;

public class MenuState : IState<GameState>
{
    public override GameState Key => GameState.MenuState;

    private readonly UnitSpawnController _unitSpawnController;
    private readonly MenuController _menuController;
    private readonly GameData _gameData;

    public MenuState(
        IStateSwitcher<GameState> stateSwitcher,
        UnitSpawnController unitSpawnController,
        MenuController menuController,
        GameData gameData) : base(stateSwitcher)
    {
        _unitSpawnController = unitSpawnController;
        _menuController = menuController;
        _gameData = gameData;
    }

    public override async UniTask Enter()
    {
        await _menuController.ShowMenu();

        _menuController.OnSimultionClickEvent += TransitionToGameLoop;
        _menuController.OnRandomizeClickEvent += _unitSpawnController.RequestRandomize;

        _unitSpawnController.Spawn(_gameData.Level.LevelConfig, _gameData.Level.SpawnPoints);
    }

    private void TransitionToGameLoop()
    {
        _stateSwitcher.SwitchState(GameState.GameLoopState);
    }

    public override async UniTask Exit()
    {
        _menuController.OnSimultionClickEvent -= TransitionToGameLoop;
        _menuController.OnRandomizeClickEvent -= _unitSpawnController.RequestRandomize;

        await _menuController.HideMenu();
    }
}
