using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnerOnlyOnce : NetworkBehaviour {
    // Tag to find the objects in the pooler
    public string poolTag;

    private void Start()
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
