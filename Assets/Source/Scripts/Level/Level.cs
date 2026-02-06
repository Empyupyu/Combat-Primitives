using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [field: SerializeField] public List<SpawnPoint> SpawnPoints {  get; private set; }
    [field: SerializeField] public LevelConfig LevelConfig {  get; private set; }
}
