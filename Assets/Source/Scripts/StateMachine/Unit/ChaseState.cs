using Cysharp.Threading.Tasks;
using UnityEngine;

public class ChaseState : IState<UnitState>
{
    public override UnitState Key => UnitState.Chase;

    private readonly UnitView _unitView;
    private readonly BattleController _battleController;

    public ChaseState(
        IStateSwitcher<UnitState> stateSwitcher, 
        UnitView unitView,
        BattleController battleController) : base(stateSwitcher)
    {
        _unitView = unitView;
        _battleController = battleController;
    }

    public override async UniTask Enter()
    {
        if(_unitView.gameObject.activeSelf)
            _unitView.Agent.isStopped = false;

        await UniTask.CompletedTask;
    }

    public override void Tick()
    {
        Chasing();
    }

    private void Chasing()
    {
        var (target, distance) = _battleController.GetClosestEnemy(_unitView);

        if(target == null)
        {
            _stateSwitcher.SwitchState(UnitState.Idle);
            return;
        }

        _unitView.SetTarget(target);

        _unitView.Agent.SetDestination(_unitView.Target.transform.position);

        if (distance <= .5f)
        {
            _stateSwitcher.SwitchState(UnitState.Attack);
        }
    }

    public override async UniTask Exit()
    {
        if (_unitView.gameObject.activeSelf)
        {
            _unitView.Agent.SetDestination(_unitView.transform.position);
            _unitView.Agent.isStopped = true;
        }

        await UniTask.CompletedTask;
    }
}
