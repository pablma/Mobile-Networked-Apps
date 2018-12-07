using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnObject : NetworkBehaviour
{
    public GameObject objectToSpawn;
    public float TimeToRespawn = 2f;
    private float timer;

    private void Update()
    {
        if (ObjectPooler.instance.isPoolerInitialized())
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ObjectPooler.instance.CmdSpawnFromPool(objectToSpawn, transform.position, transform.rotation);
                timer = TimeToRespawn;
            }
        }
    }


    //[ClientRpc]
    //private void RpcSpawn()
    //{

    //}
}