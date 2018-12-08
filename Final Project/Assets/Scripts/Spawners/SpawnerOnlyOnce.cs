using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnerOnlyOnce : NetworkBehaviour {

    // Time rate of the spawned objects
    public float TimeToRespawn = 2f;

    // Internal timer
    private float timer;

    // Tag to find the objects in the pooler
    public string poolTag;


    public override void OnStartServer()
    {
        CmdSpawn();
    }

    // Method to spawn a gameobject from the pooler
    [Command]
    void CmdSpawn()
    {
        Pool.instance.updateTagObjectFinder(poolTag);
        var obj = Pool.instance.GetFromPool(transform.position);
        NetworkServer.Spawn(obj, Pool.instance.assetId);
    }
}
