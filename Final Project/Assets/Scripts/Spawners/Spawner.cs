using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

    public float TimeToRespawn = 2f;
    private float timer;
    private bool serverHasStarted = false;


    public override void OnStartServer()
    {
        serverHasStarted = true;
    }

    // Use this for initialization
    void Start () {

	}

    private void Update()
    {
        //if (Pool.instance.poolerInitialized)
        //{
        //    //pool = GameObject.Find("Pool").GetComponent<Pool>();
        //    //if (isServer)
        //    //    ServerSpawn();
        //    //else
        //    //CmdSpawn();

        //    CmdServerSpawn();
        //}

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
        var duck = Pool.instance.GetFromPool(transform.position);
        NetworkServer.Spawn(duck, Pool.instance.assetId);
    }

    [Command]
    void CmdServerSpawn()
    {
        //var duck = Pool.instance.GetFromPool(transform.position);
        //NetworkServer.Spawn(duck, Pool.instance.assetId);
        //Pool.instance.GetFromPool(transform.position);
        NetworkServer.Spawn(Pool.instance.SpawnObject(transform.position, Pool.instance.assetId), Pool.instance.assetId);
    }
}
