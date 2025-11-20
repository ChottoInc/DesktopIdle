using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<ObjectPool> pools = new List<ObjectPool>();

    private Dictionary<string, ObjectPool> poolLookup;

    public static PoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitializePools();
    }

    private void InitializePools()
    {
        poolLookup = new Dictionary<string, ObjectPool>();

        foreach (var pool in pools)
        {
            pool.Initialize(transform);
            poolLookup.Add(pool.poolName, pool);
        }
    }

    public GameObject Pull(string poolName)
    {
        return poolLookup[poolName].Pull();
    }

    public void Return(GameObject obj, string poolName)
    {
        poolLookup[poolName].Return(obj);
    }
}
