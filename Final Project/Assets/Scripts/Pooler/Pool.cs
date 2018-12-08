using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Pool : NetworkBehaviour {

    public int m_ObjectPoolSize = 10;
    public GameObject m_Prefab;
    public GameObject[] m_Pool;

    public NetworkHash128 assetId { get; set; }

    // Handles requests to spawn GameObjects on the client
    public delegate GameObject SpawnDelegate(Vector3 position, NetworkHash128 assetId);

    // Handles requests to unspawn GameObjects on the client
    public delegate void UnSpawnDelegate(GameObject spawned);

    public static Pool instance;

    public bool poolerInitialized = false;
    public bool serverHasStarted = false;


    private void Awake()
    {
        instance = this;

        assetId = m_Prefab.GetComponent<NetworkIdentity>().assetId;
        m_Pool = new GameObject[m_ObjectPoolSize];
        for (int i = 0; i < m_ObjectPoolSize; ++i)
        {
            //m_Pool[i] = (GameObject)Instantiate(m_Prefab, Vector3.zero, Quaternion.identity);
            //m_Pool[i].name = "PoolObject" + i;
            //m_Pool[i].SetActive(false);
            CreatePooler(m_Pool, i);
        }

        ClientScene.RegisterSpawnHandler(assetId, SpawnObject, UnSpawnObject);

        poolerInitialized = true;
    }


    private void CreatePooler(GameObject [] pool, int i)
    {
        pool[i] = (GameObject)Instantiate(m_Prefab, Vector3.zero, Quaternion.identity);
        pool[i].name = "PoolObject" + i;
        pool[i].SetActive(false);
    }

    public override void OnStartServer()
    {
        serverHasStarted = true;
    }

    public GameObject GetFromPool(Vector3 position)
    {
        foreach (var obj in m_Pool)
        {
            if (!obj.activeInHierarchy)
            {
                //Debug.Log("Activating GameObject " + obj.name + " at " + position);
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }
        }
        Debug.Log("Could not grab GameObject from pool, nothing available");
        return null;
    }

    public GameObject SpawnObject(Vector3 position, NetworkHash128 assetId)
    {
        return GetFromPool(position);
    }

    public void UnSpawnObject(GameObject spawned)
    {
        //Debug.Log("Re-pooling GameObject " + spawned.name);
        spawned.SetActive(false);
    }
}
