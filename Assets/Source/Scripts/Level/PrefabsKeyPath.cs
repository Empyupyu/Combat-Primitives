using UnityEngine;

[CreateAssetMenu(menuName = "Configs/PrefabsKeyPath")]
public class PrefabsKeyPath : ScriptableObject
{
    [field: SerializeField] public string MenuUIViewKey { get; private set; }
    [field: SerializeField] public string LevelKey { get; private set; }
}