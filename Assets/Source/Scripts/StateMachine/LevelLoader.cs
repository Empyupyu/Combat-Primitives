using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LevelLoader
{
    private readonly PrefabsKeyPath _prefabsKeyPath;
    private readonly IAssetLoader _assetLoader;
    private readonly GameData _gameData;
    private readonly DiContainer _diContainer;

    public LevelLoader(PrefabsKeyPath prefabsKeyPath,
        IAssetLoader assetLoader,
        GameData gameData,
        DiContainer diContainer)
    {
        _prefabsKeyPath = prefabsKeyPath;
        _assetLoader = assetLoader;
        _gameData = gameData;
        _diContainer = diContainer;
    }

    public async UniTask LoadLevel()
    {
        GameObject levelGO = await _assetLoader.LoadPrefab(_prefabsKeyPath.LevelKey);
        _diContainer.InjectGameObject(levelGO);
        _gameData.Level = levelGO.GetComponent<Level>();
    }

    public async UniTask Unload()
    {
        await _assetLoader.Unload(_prefabsKeyPath.LevelKey);
    }
}
