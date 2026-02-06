using Zenject;

public class GameStartInitializer : IInitializable
{
    private readonly GameStateMachineMono _switcher;

    public GameStartInitializer(GameStateMachineMono switcher)
    {
        _switcher = switcher;
    }

    public void Initialize()
    {
        _switcher.SwitchState(GameState.BootState);
    }
}
