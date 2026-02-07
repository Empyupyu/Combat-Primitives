using Cysharp.Threading.Tasks;

public class IdleState : IState<UnitState>
{
    public override UnitState Key => UnitState.Idle;

    public IdleState(IStateSwitcher<UnitState> stateSwitcher) : base(stateSwitcher)
    {
    }

    public override async UniTask Enter()
    {
        await UniTask.CompletedTask;
    }

    public override async UniTask Exit()
    {
        await UniTask.CompletedTask;
    }
}
