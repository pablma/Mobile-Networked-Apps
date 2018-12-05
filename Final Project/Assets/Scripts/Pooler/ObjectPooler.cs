using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectPooler : MonoBehaviour {

    // A class to create pools of different objects
    [System.Serializable]
    public class objectPoolItem
    {
        [Tooltip("This tag is only informative for the designer to keep an order")]
        public string tag;
        public GameObject prefabToPool;
        public int amountToPool;
    }

    // A list to store all of our different types of items
    public List<objectPoolItem> itemsToPool;

    // New dictionary set to be able to find a specific pool of objects
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // We make the pool a singleton to get access in an easy way
    public static ObjectPooler instance;

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
                GameObject go = Instantiate(item.prefabToPool);
                go.SetActive(false);
                objectPool.Enqueue(go);
                NetworkServer.Spawn(go);
            }

            // We add the pool to the dictionary
            poolDictionary.Add(item.prefabToPool.name, objectPool);
        }
    }

    // Method to get an item from one of the pools
    private GameObject getItemFromPool(GameObject go)
    {
        // To prevent unexpected errors
        if (!poolDictionary.ContainsKey(go.name))
        {
            Debug.LogWarning("GameObject '" + go.name + "' doesn't exist.");
            return null;
        }

        // We search the pool and select the first element
        GameObject objectToSpawn = poolDictionary[go.name].Dequeue();

        // We add the element selected to the back to reuse it later
        poolDictionary[go.name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    // Method to spawn a gameObject from one of the pools
    public void spawnFromPool(GameObject go, Vector2 position, Quaternion rotation)
    {
        // We search the pool and give life to one of the objects
        GameObject obj = getItemFromPool(go);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // We call an specific method of an interface to make sure the start method of the reused objects works 
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();

        if (pooledObj != null)
            pooledObj.OnObjectSpawn();
    }

    // Method to disabled a gameObject form one of the pools
    public void killGameObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    // Checks if there's a pool with an specific tag
    public bool itemExists(GameObject go)
    {
        bool b;

        if (poolDictionary.ContainsKey(go.name))
            b = true;
        else
            b = false;

        return b;
    }
}
