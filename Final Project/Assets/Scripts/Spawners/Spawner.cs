using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

    // Time rate of the spawned objects
    public float TimeToRespawn = 2f;

    // Internal timer
    private float timer;

    // Boolean to know when the server has started
    private bool serverHasStarted = false;

    // Tag to find the objects in the pooler
    public string poolTag;

    // To know when the server has started
    public override void OnStartServer()
    {
        serverHasStarted = true;
    }

    private void Update()
    {
        // If the server has started, we applied the timer logic to spawn objects every 'x' seconds
        if (serverHasStarted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                CmdSpawn();
                timer = TimeToRespawn;
            }
        }
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
