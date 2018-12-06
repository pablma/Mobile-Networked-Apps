using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {

    public static GameManager instance;

    public Text player1PointsText;
    public Text player2PointsText;

    public Image player1PanelColor;
    public Image player2PanelColor;
    //[SyncVar(hook = "updateUi1")] 
    [SyncVar]
    public int player1Points = 0;

    //[SyncVar(hook = "updateUi2")]
    [SyncVar]
    public int player2Points = 0;

    GameObject[] players;
	// Use this for initialization
	void Start () {
        instance = this;

        player1PointsText.text = player1Points.ToString();
        player2PointsText.text = player2Points.ToString();

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

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    //allow us to call the shoot function from the the canvas, from the player (FinguerGun) script it will be decided wich one is the local player shooting
    public void shoot()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<FingerGun>().Shoot();
        }
    }

    public void givePoints(int duckId, int bulletId)
    {
        switch (bulletId)
        {
            case 0:
                player1Points += duckId;
                break;
            case 1:
                player2Points += duckId;
                break;
        }

        player1PointsText.text = player1Points.ToString();
        player2PointsText.text = player2Points.ToString();
    }

    public void setPlayer1PanelColor(Color c)
    {
        player1PanelColor.color = c;
    }

    public void setPlayer2PanelColor(Color c)
    {
        player2PanelColor.color = c;
    }

    //void updateUi1(int player1Points)
    //{
    //    //player1PointsText.text = player1Points.ToString();
    //}

    //void updateUi2(int player2Points)
    //{
    //    //player2PointsText.text = player2Points.ToString();
    //}
}
