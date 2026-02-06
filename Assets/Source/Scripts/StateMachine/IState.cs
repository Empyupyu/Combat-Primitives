using Cysharp.Threading.Tasks;
using System;

public abstract class IState<T> where T : Enum
{
    public abstract T Key { get; }
    protected readonly IStateSwitcher<T> _stateSwitcher;

    public IState(IStateSwitcher<T> stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public abstract UniTask Enter();
    public abstract UniTask Exit();
    public virtual void Tick() { }
}
