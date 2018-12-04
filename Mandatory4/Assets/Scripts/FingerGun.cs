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
    public int bulletVel = 6;
    public int bulletID = 0;

    GameObject[] players;
    private Camera camera;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            bulletID = 1;
        }
        else bulletID = 0;
    }
    void Start()
    {
        if ((GameObject.Find("Main Camera") != null) && (GameObject.Find("Main Camera").GetComponent<Camera>() != null))
        {
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }



        //if (isLocalPlayer)
        //{
        //    Debug.Log("LocalPlayerPrimero");
        //}
        //else
        //    Debug.Log("OtherPlayerSegundo");



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

    [Command]
    public void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position, bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletVel;
        bullet.GetComponent<Bullet>().GiveName(bulletID);
        //Allows to instantiate game objects on server in order to be instatiate in all the clients of the server
        NetworkServer.Spawn(bullet);


        // Destroy the bullet after 2 seconds
        Destroy(bullet, 20.0f);
    }
}
