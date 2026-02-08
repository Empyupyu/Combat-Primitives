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

        Agent.speed = Unit.Stats.SPEED;
        Agent.speed *= Constants.UnitSpeedMultiply;

        Unit.OnDie += HandleDeath;
        Unit.OnHealthChange += HandleHealthChanged;
    }

    public void SetShape(ShapeView shape)
    {
        Shape = shape;
    }

    public void SetColor(Color color)
    {
        Shape.MeshRenderer.material.SetFloat("_Fill_Height", 1);
        Shape.MeshRenderer.material.SetColor("_Fill_Color", color);
    }

    public void SetTarget(UnitView target)
    {
        Target = target;
    }

    private void HandleHealthChanged(float current, float max)
    {
        float normalized = current / max;
        float shaderValue = normalized * 2f - 1f;

        Shape.MeshRenderer.material.SetFloat("_Fill_Height", shaderValue);
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
            Unit.OnHealthChange -= HandleHealthChanged;
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
        {
            Unit.OnDie -= HandleDeath;
            Unit.OnHealthChange -= HandleHealthChanged;
        }
    }
}
