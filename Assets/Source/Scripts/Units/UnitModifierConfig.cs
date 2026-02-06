using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UnitStatsModifiers")]
public class UnitModifierConfig : ScriptableObject
{
    [field: SerializeField] public UnitBaseStatsConfig UnitBaseConfig { get; private set; }
    [field: SerializeField] public List<UnitShape> Shapes { get; private set; }
    [field: SerializeField] public List<UnitSize> Sizes { get; private set; }
    [field: SerializeField] public List<UnitColor> Colors { get; private set; }
}

