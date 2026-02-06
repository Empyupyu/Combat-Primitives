using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IAssetLoader
{
    UniTask<GameObject> LoadPrefab(string key);
    UniTask Unload(string key);
    void UnloadAll();
}
