using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pool : NetworkBehaviour {
    
    // A class to create pools of different objects
    [System.Serializable]
    public class objectPoolItem
    {
        public string tag;
        public GameObject objectToPool;
        public int amountToPool;
    }

    // A list to store all of our different types of items
    public List<objectPoolItem> itemsToPool;

    // New dictionary set to be able to find a specific pool of objects
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    //public int m_ObjectPoolSize = 10;
    //public GameObject m_Prefab;
    //GameObject[] m_Pool;

    public NetworkHash128 assetId { get; set; }

    // Handles requests to spawn GameObjects on the client
    public delegate GameObject SpawnDelegate(Vector3 position, NetworkHash128 assetId);

    // Handles requests to unspawn GameObjects on the client
    public delegate void UnSpawnDelegate(GameObject spawned);

    // We make the pool a singleton to get access in an easy way
    public static Pool instance;

    private string TagObjectFinder;

    private void Awake()
    {
        instance = this;

        // We create a new dictionary
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (objectPoolItem item in itemsToPool)
        {
            // We create a gameObject queue for each key of the dictionary
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // We add the objects to the pools
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject go = Instantiate(item.objectToPool);
                go.SetActive(false);
                objectPool.Enqueue(go);
            }

            // We add the pool to the dictionary
            poolDictionary.Add(item.tag, objectPool);

            ClientScene.RegisterSpawnHandler(assetId, SpawnObject, UnSpawnObject);
        }

        //Pool
        //assetId = m_Prefab.GetComponent<NetworkIdentity>().assetId;
        //m_Pool = new GameObject[m_ObjectPoolSize];
        //for (int i = 0; i < m_ObjectPoolSize; ++i)
        //{
        //    m_Pool[i] = (GameObject)Instantiate(m_Prefab, Vector3.zero, Quaternion.identity);
        //    m_Pool[i].name = "PoolObject" + i;
        //    m_Pool[i].SetActive(false);
        //}

        //ClientScene.RegisterSpawnHandler(assetId, SpawnObject, UnSpawnObject);
    }

    private GameObject findObjectInPool(string tag, Vector3 position)
    {
        // To prevent unexpected errors
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("GameObject with tag '" + tag + "' doesn't exist.");
            return null;
        }

        // We search the pool and select the first element
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        // We add the element selected to the back to reuse it later
        poolDictionary[tag].Enqueue(objectToSpawn);

        // We modify the fields of the gameObject
        objectToSpawn.transform.position = position;
        objectToSpawn.SetActive(true);

        return objectToSpawn;

        //Pool
        //foreach (var obj in m_Pool)
        //{
        //    if (!obj.activeInHierarchy)
        //    {
        //        obj.transform.position = position;
        //        obj.SetActive(true);
        //        return obj;
        //    }
        //}
        //Debug.Log("Could not grab GameObject from pool, nothing available");
        //return null;
    }

    public GameObject GetFromPool(Vector3 position)
    {
        return findObjectInPool(TagObjectFinder, position);
    }

    public GameObject SpawnObject(Vector3 position, NetworkHash128 assetId)
    {
        return findObjectInPool(TagObjectFinder, position);
    }

    public void UnSpawnObject(GameObject spawned)
    {
        spawned.SetActive(false);
    }

    public void updateTagObjectFinder(string t)
    {
        TagObjectFinder = t;
    }
}
