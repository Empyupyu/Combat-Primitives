using Cysharp.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

public class AttackState : IState<UnitState>
{
    public override UnitState Key => UnitState.Attack;

    private readonly UnitView _unitView;
    private readonly DamageService _damageService;
    private bool _availableAttack = true;
    private CancellationTokenSource _cts;

    public AttackState(
        IStateSwitcher<UnitState> stateSwitcher,
        UnitView unitView,
        DamageService damageService) : base(stateSwitcher)
    {
        _unitView = unitView;
        _damageService = damageService;
    }

    public override async UniTask Enter()
    {
        _cts = new CancellationTokenSource();

        await AttackRoutine(_cts.Token);
    }

    private async UniTask AttackRoutine(CancellationToken token)
    {
        if (!_availableAttack)
        {
            await UniTask.Delay((int)(_unitView.Unit.Stats.ATKSPD * 200), cancellationToken: token);
            _availableAttack = true;
        }

        if (token.IsCancellationRequested)
            return;

        if (_unitView.Unit == null || _unitView.Target == null)
            return;

        _damageService.ApplyDamage(_unitView.Target, _unitView.Unit.Stats.ATK);

        _availableAttack = false;

        if (_unitView.gameObject.activeSelf)
            _unitView.Agent.isStopped = true;

        if (!token.IsCancellationRequested)
            await _stateSwitcher.SwitchState(UnitState.Chase);
    }


    public override async UniTask Exit()
    {
        _cts?.Cancel();
        await UniTask.CompletedTask;
    }
}
