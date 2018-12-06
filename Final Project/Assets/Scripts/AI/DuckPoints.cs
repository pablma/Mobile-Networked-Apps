using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckPoints : MonoBehaviour {

    public int points = 0;
    private LobbyBullet lobbyBullet;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {

        lobbyBullet = collision.gameObject.GetComponent<LobbyBullet>();
        if (lobbyBullet != null)
        {
            GameManager.instance.givePoints(points,lobbyBullet.getId());
        }
    }


}
