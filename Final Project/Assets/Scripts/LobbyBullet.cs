using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyBullet : NetworkBehaviour {

    [SyncVar]
    public int bulletId = 21;

    private void Start()
    {
        if (bulletId == 0)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = GameManager.instance.giveP1Color();
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = GameManager.instance.giveP2Color();
        }
    }
    private void Update()
    {

        //Allow us to distinguish the two bullets
    }

    void OnCollisionEnter(Collision collision) 
    {
        //the bullets are killed on collision
        //Destroy(gameObject //we call the method that allow us to disable (kill) the object the bullet is colliding that will be reused by the pool.
        //the bullet will get the component 
    }

    public void assingId(int ID)
    {
        bulletId = ID;
    }

    public int getId()
    {
        return bulletId;
    }
}
