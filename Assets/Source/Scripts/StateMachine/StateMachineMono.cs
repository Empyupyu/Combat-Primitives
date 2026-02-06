using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class StateMachineMono<T> : MonoBehaviour, IStateSwitcher<T> where T : Enum
{
    protected StateMachine<T> _stateMachine;

    public void SetStateMachine(StateMachine<T> stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public UniTask SwitchState(T stateName)
    {
        return _stateMachine.SwitchState(stateName);
    }

    public void Tick()
    {
        _stateMachine?.Tick();
    }
}
