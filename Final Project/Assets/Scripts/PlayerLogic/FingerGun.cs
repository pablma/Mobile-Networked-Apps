using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class FingerGun : NetworkBehaviour
{
    //It is the one that has the player logic.


    public GameObject bulletPrefab; //gO that is going to be spawned

    public Transform bulletSpawn; //position from where the bullet is going to be instantiate

    public int bulletVel = 6;//speed of the spawnable bullet
    public int playerId = 0;

    GameObject[] players;// allow us to find all the player on the scene to identificate each of them
    //private Camera camera;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");//get the players on scene and store them on an array

        if (players.Length > 1)
        {
            playerId = 1;
        }
        else playerId = 0;
        //a number to identificate the player, the bullet spawned by each one is coing to have the same identificator
    }

    void Start()
    {

        GameManager.instance.UpdatePlayersArray();
        IdentifyPlayersPanel();
        //if ((GameObject.Find("Main Camera") != null) && (GameObject.Find("Main Camera").GetComponent<Camera>() != null))
        //{
        //    camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //}
    }

    // Update is called once per frame
    void Update () {
    }

    void IdentifyPlayersPanel()
    {
        //allow us to send to the GameManager the information about the color selectod for each player to color their respective panels
        if (playerId == 0)
        {

            GameManager.instance.setPlayer1PanelColor(GetComponentInChildren<PlayerColor>().pColor);
        }
        else
            GameManager.instance.setPlayer2PanelColor(GetComponentInChildren<PlayerColor>().pColor);
    }


    public void Shoot()//public function that calls the private and Cmd function,
    {
        //it allow us to call it from the canvas to make posible the button shotting

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
            GetComponentInChildren<Camera>().enabled = true; // when the server starts we give preference to the camera of the local player
    }

    [Command]
    public void CmdFire()
    {
        // uses the bullet from the pool

        Pool.instance.updateTagObjectFinder("Bullet");
        var bullet = Pool.instance.GetFromPool(bulletSpawn.position);

        // Add velocity to the bullet
        bullet.transform.rotation = bulletSpawn.rotation;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletVel;
        bullet.GetComponent<LobbyBullet>().assingId(playerId);//gives the bullet the same Id than the player that is shooting, to identificate the owner.

        //Allows to instantiate game objects on server in order to be instatiate in all the clients of the server
        NetworkServer.Spawn(bullet, Pool.instance.assetId);

        // Despown the bullet after 5 seconds if there have been no collition with ducks or cans or other

        StartCoroutine(KillObjectOnTime(bullet, 5.0f));
    }

    public IEnumerator KillObjectOnTime(GameObject go, float timer)//courtrutine that allow us to unspawn a bullet if it has not collide witha duck or a can
    {
        yield return new WaitForSeconds(timer);
        Pool.instance.UnSpawnObject(go);
        NetworkServer.UnSpawn(go);
    }

    public void ThereAreAWinner(int winnerId)//updates the canvas to show the correct message of the player state when the game is over
    {
        string state = "There are no winners at war";
        if (isLocalPlayer)
        {
            if(winnerId == playerId)
            {
                state = "You Won!";
            }
            else
                state = "You lose!";
                    GameManager.instance.setGameOverText(state);
        }
    }

    public int getPlayerId()//allow us to get the player id to identificate him
    {
        return playerId;
    }
}
