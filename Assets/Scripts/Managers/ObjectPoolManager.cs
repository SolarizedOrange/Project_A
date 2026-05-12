using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;

[AttributeUsage(AttributeTargets.Field)]
public class PoolAttribute : Attribute { }

public static class PoolExtension
{
    public static GameObject Spawn(this GameObject origin, GameObject prefab, Vector3 position = default, Quaternion rotation = default)
    {
        return ObjectPoolManager.Instance.GetObject(prefab, position, rotation);
    }

    public static void Despawn(this GameObject origin, GameObject prefab)
    {
        ObjectPoolManager.Instance.ReturnObject(prefab);
    }
}

public class ObjectPoolManager : ManagerBase<ObjectPoolManager>
{
    public List<GameObject> PoolPrefabs = new ();
    Dictionary<GameObject, IObjectPool<GameObject>> pools = new ();
	protected override void Awake()
	{
        base.Awake();

        RuntimeAutoScan();

		foreach (var item in PoolPrefabs)
        {
            CreateObject(item);
        }
	}

    private void RuntimeAutoScan()
    {
        MonoBehaviour[] allScripts = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        HashSet<GameObject> targetPrefabs = new HashSet<GameObject>();

        foreach (var script in allScripts)
        {
            if (script == null) continue;

            // Find Pool Fields via Reflection
            var fields = script.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            foreach (var field in fields)
            {
                if (Attribute.IsDefined(field, typeof(PoolAttribute)))
                {
                    GameObject prefab = field.GetValue(script) as GameObject;
                    if (prefab != null) targetPrefabs.Add(prefab);
                }
            }
        }

        foreach (var prefab in targetPrefabs)
        {
            PoolPrefabs.Add(prefab);
        }
    }

    void CreateObject(GameObject prefab)
    {
        if (pools.ContainsKey(prefab)) return;
        
        var objectPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
        pools.Add(prefab, objectPool);
    }

    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefab))
        {
            CreateObject(prefab);
        }
        var obj = pools[prefab].Get();
        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    public void ReturnObject(GameObject prefab)
    {
        if (pools.TryGetValue(prefab, out var pool))
        {
            pool.Release(prefab);
        }
        else
        {
            Destroy(prefab);
        }
    }

}
