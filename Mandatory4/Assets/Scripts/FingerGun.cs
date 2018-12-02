using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class FingerGun : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private Camera camera;

    void Start()
    {
        if ((GameObject.Find("Main Camera") != null) && (GameObject.Find("Main Camera").GetComponent<Camera>() != null))
        {
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
        {
            //changes the collor to red for not local player (the other player)
            GetComponentInChildren< MeshRenderer > ().material.color = Color.red;
            return;
        }
    }


    public void Shoot()
    {
        if (isLocalPlayer)
        {
            CmdFire();
        }
    }

    public override void OnStartLocalPlayer()
    {        
        base.OnStartLocalPlayer();
        {
        }
        //changes the color for the local player
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;

        GetComponentInChildren<Camera>().enabled = true;
    }

    public override void OnStartClient()
    {
        //base.OnStartClient();

        //for (int i = 0; i < cameras.Length; i++)
        //{
        //    if(isLocalPlayer)
        //    cameras[i].enabled = true;
        //    else cameras[i].enabled = false;

        //}

        //Debug.Log(cameras.Length);
    }


    [Command]
    public void CmdFire()
    {
        
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position, bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
        //Allows to instantiate game objects on server in order to be instatiate in all the clients of the server
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 20.0f);
    }


}
