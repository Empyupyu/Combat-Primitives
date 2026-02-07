using Cysharp.Threading.Tasks;
using Zenject;

public class GameStateMachineMono : StateMachineMono<GameState>
{
    [Inject]
    public void Construct(StateMachine<GameState> machine)
    {
        SetStateMachine(machine);
        _stateMachine.SwitchState(GameState.BootState).Forget();
    }
}
