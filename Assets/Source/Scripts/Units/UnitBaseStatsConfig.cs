using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UnitBaseStatsConfig")]
public class UnitBaseStatsConfig : ScriptableObject
{
    [field: SerializeField] public float HP { get; private set; }
    [field: SerializeField] public float ATK { get; private set; }
    [field: SerializeField] public float SPEED { get; private set; }
    [field: SerializeField] public float ATKSPD { get; private set; }
}

