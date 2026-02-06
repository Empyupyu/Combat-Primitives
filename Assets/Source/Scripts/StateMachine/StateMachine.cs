using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

public class StateMachine<T> where T : Enum
{
    private Dictionary<T, IState<T>> _states;
    private IState<T> _currentState;

    public StateMachine(List<IState<T>> states)
    {
        _states = states.ToDictionary(s => s.Key, s => s);
    }

    public void AddState(T stateName, IState<T> state)
    {
        if( _states == null)
        {
            _states = new Dictionary<T, IState<T>>();
        }

        if (!_states.ContainsKey(stateName))
        {
            _states.Add(stateName, state);
        }
        else
        {
            throw new InvalidOperationException($"State '{stateName}' is already added to the state machine.");
        }
    }

    public async UniTask SwitchState(T state)
    {
        if (!_states.TryGetValue(state, out var newState)) 
            throw new InvalidOperationException($"State '{state}' not found.");

        if(_currentState != null)
        {
            await _currentState.Exit();
        }

        _currentState = newState;
        await _currentState.Enter();
    }

    public void Tick()
    {
        _currentState?.Tick();
    }
}
