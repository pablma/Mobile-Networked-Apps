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
            //////////////Destroy(lobbyBullet.gameObject);
        }
    }


}
