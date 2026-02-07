using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly List<T> _pool;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;

        if (parent == null)
        {
            GameObject parentGO = new GameObject(typeof(T).Name + "_Pool");
            parentGO.transform.position = Vector3.zero;
            _parent = parentGO.transform;
        }
        else
        {
            _parent = parent;
        }

        _pool = new List<T>(initialSize);

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(_prefab, _parent);
        obj.gameObject.SetActive(false);
        _pool.Add(obj);
        return obj;
    }

    public T Get()
    {
        foreach (var obj in _pool)
        {
            if (!obj.gameObject.activeSelf)
            {
                return obj;
            }
        }
        return CreateNewObject();
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_parent);
    }
}
