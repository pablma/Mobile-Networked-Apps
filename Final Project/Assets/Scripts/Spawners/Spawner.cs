using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

    public float TimeToRespawn = 2f;
    private float timer;
    private bool serverHasStarted = false;
    public string poolTag;


    public override void OnStartServer()
    {
        serverHasStarted = true;
    }

    // Use this for initialization
    void Start () {

	}

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //    CmdSpawn();

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

    [Command]
    void CmdSpawn()
    {
        Pool.instance.updateTagObjectFinder(poolTag);
        var duck = Pool.instance.GetFromPool(transform.position);
        NetworkServer.Spawn(duck, Pool.instance.assetId);
    }

}
