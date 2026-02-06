using UnityEngine;
using UnityEngine.AI;

public class UnitView : MonoBehaviour
{
    [field: SerializeField] public GameObject Model {  get; private set; }
    [field: SerializeField] public Unit Unit {  get; private set; }
    [field: SerializeField] public NavMeshAgent Agent {  get; private set; }

    public void SetUnit(Unit unit)
    {
        Unit = unit;
    }

    public void SetModel(GameObject model)
    {
        Model = model;
    }
}

