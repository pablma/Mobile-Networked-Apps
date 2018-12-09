using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DuckPoints : NetworkBehaviour {

    public int points = 0;
    private LobbyBullet lobbyBullet;

    Points[] gM;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        //when the buller collides it gives the number of point seted on the object to the owner of the bullet that has collide with it
        gM = FindObjectsOfType<Points>();

        lobbyBullet = collision.gameObject.GetComponent<LobbyBullet>();
        if (lobbyBullet != null)
        {
            for(int i = 0; i < gM.Length; i++)
            {
                if(lobbyBullet.getId() == gM[i].GetPlayerId())
                {
                    gM[i].giveMePoints(points);
                }
            }

            Pool.instance.UnSpawnObject(lobbyBullet.gameObject);
            NetworkServer.UnSpawn(lobbyBullet.gameObject);
        }
    }


}
