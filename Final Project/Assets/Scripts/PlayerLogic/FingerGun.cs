using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class FingerGun : NetworkBehaviour
{
    public GameObject bulletPrefab; //gO that is going to be spawned

    public Transform bulletSpawn; //position from where the bullet is going to be instantiate

    public int bulletVel = 6;//speed of the spawnable bullet
    public int playerId = 0;

    GameObject[] players;// allow us to find all the player on the scene to identificate each of them
    //private Camera camera;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            playerId = 1;
        }
        else playerId = 0;
        //a number to identificate the player, the bullet spawned by each one is coing to have the same identificator
    }
    void Start()
    {

        //if (playerId == 0)
        //{
        //    //changes the collor to red for the not local player (the other player)
        //    GetComponentInChildren<MeshRenderer>().material.color = Color.blue; ;
        //}
        //else
        //    GetComponentInChildren<MeshRenderer>().material.color = Color.red;

        //if ((GameObject.Find("Main Camera") != null) && (GameObject.Find("Main Camera").GetComponent<Camera>() != null))
        //{
        //    camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //}
    }

    // Update is called once per frame
    void Update () {
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
        bullet.GetComponent<LobbyBullet>().assingId(playerId);//gives the bullet the same Id than the player that is shooting, to identificate the owner.

        //Allows to instantiate game objects on server in order to be instatiate in all the clients of the server
        NetworkServer.Spawn(bullet);


        // Destroy the bullet after 2 seconds
        Destroy(bullet, 5.0f);
    }
}
