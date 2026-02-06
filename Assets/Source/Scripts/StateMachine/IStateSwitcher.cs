using Cysharp.Threading.Tasks;
using System;

public interface IStateSwitcher<T> where T : Enum
{
    public UniTask SwitchState(T stateName);
}
