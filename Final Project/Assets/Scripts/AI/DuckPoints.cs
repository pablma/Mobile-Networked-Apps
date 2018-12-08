using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckPoints : MonoBehaviour {

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
            //////////GameManager.instance.givePoints(points,lobbyBullet.getId());


            for(int i = 0; i < gM.Length; i++)
            {
                if(lobbyBullet.getId() == gM[i].GetPlayerId())
                {
                    gM[i].giveMePoints(points);
                }
            }
        }
    }


}
