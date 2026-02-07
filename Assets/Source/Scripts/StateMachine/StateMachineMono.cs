using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class StateMachineMono<T> : MonoBehaviour, IStateSwitcher<T> where T : Enum
{
    public bool IsInitialized { get; private set; }
    protected StateMachine<T> _stateMachine;

    public void SetStateMachine(StateMachine<T> stateMachine)
    {
        _stateMachine = stateMachine;
        IsInitialized = true;
    }

    public UniTask SwitchState(T stateName)
    {
        return _stateMachine.SwitchState(stateName);
    }

    private void Update()
    {
        _stateMachine?.Tick();
    }
}
