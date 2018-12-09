using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyBullet : NetworkBehaviour {

    [SyncVar]
    public int bulletId = 21;//alow us to sincronice the bullet id

    private void Start()
    {
        IdentificateBullets();
    }

    private void Update()
    {
    }

    
    void IdentificateBullets()//allow us to identificate the bullets by his owner color, the bullet and the owner will have the same color
    {
        if (bulletId == 0)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = GameManager.instance.GetP1Color();
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = GameManager.instance.GetP2Color();
        }
    }

    public void assingId(int ID)//allow us to assing the id of the bullet wich will be the same as the player that is shooting
    {
        bulletId = ID;
    }

    public int getId()//allow us to get from other script the id of each bullet, it allow us to know who is the owner of each bullet
    {
        return bulletId;
    }

    private void OnCollisionEnter(Collision collision)//Unspawn the object when collides with another object
    {
        Pool.instance.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }
}
