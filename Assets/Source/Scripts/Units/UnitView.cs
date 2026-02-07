using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitView : MonoBehaviour
{
    public event Action<UnitView> OnViewDestroyed;

    [field: SerializeField] public ShapeView Shape {  get; private set; }
    [field: SerializeField] public Unit Unit {  get; private set; }
    [field: SerializeField] public NavMeshAgent Agent {  get; private set; }
    [field: SerializeField] public UnitStateMachineMono UnitStateMachineMono {  get; private set; }
    public UnitView Target { get; private set; }

    public void SetUnit(Unit unit)
    {
        Unit = unit;
        float fakeSpeedMultiply = .3f;

        Agent.speed = Unit.Stats.SPEED;
        Agent.speed *= fakeSpeedMultiply;

        Unit.OnDie += HandleDeath;
    }

    public void SetShape(ShapeView shape)
    {
        Shape = shape;
    }

    public void SetColor(Material color)
    {
        Shape.MeshRenderer.material = color;
    }

    public void SetTarget(UnitView target)
    {
        Target = target;
    }

    private void HandleDeath(Unit unit) 
    {
        Unit.OnDie -= HandleDeath;
        OnViewDestroyed?.Invoke(this);
    }

    public void ResetView()
    {
        Target = null;

        if (Unit != null)
        {
            Unit.OnDie -= HandleDeath;
            Unit = null;
        }

        if (UnitStateMachineMono != null && UnitStateMachineMono.IsInitialized)
        {
            UnitStateMachineMono.SwitchState(UnitState.Idle);
        }

        if (Shape != null)
        {
            Shape.gameObject.SetActive(false);
            Shape = null;
        }

    }

    private void OnDisable()
    {
        if(Unit != null)
            Unit.OnDie -= HandleDeath;
    }
}
