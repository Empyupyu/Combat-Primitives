public class GameStateMachineMono : StateMachineMono<GameState>
{
    private void Update()
    {
        _stateMachine?.Tick();
    }
}
