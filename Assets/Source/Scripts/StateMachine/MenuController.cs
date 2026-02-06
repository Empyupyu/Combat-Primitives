using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class MenuController
{
    public event Action OnSimultionClickEvent;
    public event Action OnRandomizeClickEvent;

    private readonly IAssetLoader _assetLoader;
    private readonly PrefabsKeyPath _prefabsKeyPath;

    private MenuUIView _menuUIViewInstance;

    public MenuController(IAssetLoader assetLoader,
        PrefabsKeyPath prefabsKeyPath)
    {
        _assetLoader = assetLoader;
        _prefabsKeyPath = prefabsKeyPath;
    }

    public async UniTask ShowMenu()
    {
        GameObject menuViewGO = await _assetLoader.LoadPrefab(_prefabsKeyPath.MenuUIViewKey);

        _menuUIViewInstance = menuViewGO.GetComponent<MenuUIView>();

        _menuUIViewInstance.RandomizeUnitSetupButton.onClick.AddListener(RandomizeClick);
        _menuUIViewInstance.SimulationButton.onClick.AddListener(SimulationClick);
    }

    private void RandomizeClick()
    {
        OnRandomizeClickEvent?.Invoke();
    }

    private void SimulationClick()
    {
        OnSimultionClickEvent?.Invoke();
    }

    public async UniTask HideMenu()
    {
        _menuUIViewInstance.RandomizeUnitSetupButton.onClick.RemoveListener(RandomizeClick);
        _menuUIViewInstance.SimulationButton.onClick.RemoveListener(SimulationClick);

        await _assetLoader.Unload(_prefabsKeyPath.MenuUIViewKey);
    }
}
