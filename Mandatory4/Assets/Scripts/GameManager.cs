using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    GameObject[] players;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void LoadShooterGame()
    {
        SceneManager.LoadScene("ShooterGame");
    }

    public void LoadTapGame()
    {
        SceneManager.LoadScene("TapGame");
    }

    public void LoadTapV2()
    {
        SceneManager.LoadScene("TapV2");
    }

    public void shoot()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<FingerGun>().dispara();
        }
    }
}
