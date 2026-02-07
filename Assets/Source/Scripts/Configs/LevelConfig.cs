using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public List<TeamSettings> Teams { get; private set; }
}
