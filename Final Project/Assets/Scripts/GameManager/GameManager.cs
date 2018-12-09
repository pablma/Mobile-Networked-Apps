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

    public int maxPoints = 100;

    public Image endPanel;
    public Text endText;

    private int player1Points = 0;
    private int player2Points = 0;

    GameObject[] players;

    bool gameOver = false;

    private void Awake()
    {
        instance = this;
    }
    void Start () {


        player1PointsText.text = player1Points.ToString();
        player2PointsText.text = player2Points.ToString();

        endText.text = "";
	}


    public void UpdatePlayersArray() // allow us to keep in this variable acces to the players on the scene
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }


    void Update () {
	}
    
    //public void LoadShooterGame()
    //{
    //    SceneManager.LoadScene("ShooterGame");
    //}

    //public void LoadTapGame()
    //{
    //    SceneManager.LoadScene("TapGame");
    //}

    //public void LoadTapV2()
    //{
    //    SceneManager.LoadScene("TapV2");
    //}

    //public void LoadMenu()
    //{
    //    SceneManager.LoadScene("Menu");
    //}

    
    public void shoot()//allow us to call the shoot function from the the canvas, from the player (FinguerGun) script it will be decided wich one is the local player shooting
    {

        if (!gameOver)//only shoots if the game is not ended
        {
            UpdatePlayersArray();
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<FingerGun>().Shoot();
            }
        }
    }

    //public void givePoints(int duckId, int bulletId)
    //{
    //    switch (bulletId)
    //    {
    //        case 0:
    //            {
    //                player1Points += duckId;
    //                if (player1Points >= maxPoints)
    //                    gameOver(0);
    //                break;
    //            }
    //        case 1:
    //            {
    //                player2Points += duckId;
    //                if (player2Points >= maxPoints)
    //                    gameOver(1);
    //                break;
    //            }
    //    }

    //    player1PointsText.text = player1Points.ToString();
    //    player2PointsText.text = player2Points.ToString();
    //}

    public void setPlayer1PanelColor(Color c) // alow us to set the panel of player 1 color on the UI to the one selected by player 1
    {
        player1PanelColor.color = c;
    }

    public void setPlayer2PanelColor(Color c)// alow us to set the panel of player 1 color on the UI to the one selected by player 1
    {
        player2PanelColor.color = c;
    }

    public Color GetP1Color()//allow us to get the color values of the playd one
    {
        return player1PanelColor.color;
    }

    public Color GetP2Color()//allow us to get the color values of the playd Two
    {
        return player2PanelColor.color;
    }



    public void updatePlayerOnePoints(int points)//updates the points the points that apear on the Uifor the player 1, will be called from FingerGun
    {                                            //wich is the one that counts the points, in this case the player 1.
        player1Points = points;
        player1PointsText.text = player1Points.ToString();
        IsPlayer1Winner();
    }

    public void updatePlayerTwoPoints(int points)//updates the points the points that apear on the Uifor the player 2, will be called from FingerGun 
    {                                             //wich is the one that counts the points, in this case the player 1.
        player2Points = points;
        player2PointsText.text = player2Points.ToString();
        IsPlayer2Winner();
    }

    void IsPlayer1Winner()//see if player1 has won and if it is so calls game object with the iD set on 0, wich is the one that represents player 1
    {
        if (player1Points >= maxPoints)
        {
            GameOver(0);
        }
    }

    void IsPlayer2Winner()//see if player2 has won and if it is so calls game object with the iD set on 1, wich is the one that represents player 2
    {
        if (player2Points >= maxPoints)
        {
            GameOver(1);
        }
    }



    void GameOver(int winnerId)//on GameOver it sends information about wich is the id of the winner to all the players (ones that own the FingerGun script)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<FingerGun>().ThereAreAWinner(winnerId);
        }
        gameOver = true;
        ActiveGameOverPanel();
    }

    public void setGameOverText(string state)//allow us to set the text that would apear for each player in his scene. It will be called from the player script (FingerGun)
    {
        endText.text = state;
    }


    void ActiveGameOverPanel()//show the state of the player (winner or loser) and activates the panel to go back to lobby
    { // we are using the LobbyTopPanel script giving by the dowloaded library that give us acces to lobby prefabs and functions. We have modify the 
        //LobbyTopPanel script to achieve show the upper panel when the Game is Over.
        Prototype.NetworkLobby.LobbyTopPanel lTP;
        lTP = FindObjectOfType<Prototype.NetworkLobby.LobbyTopPanel>();
        if (lTP != null)
        {
            bool lobbyTopPanelState = lTP.LobbyTopPanelState();
            lTP.ToggleVisibility(!lobbyTopPanelState);
        }
        endPanel.gameObject.SetActive(true);
    }
    //public void backToLobby()
    //{
    //    FindObjectOfType<Prototype.NetworkLobby.LobbyManager>().backDelegate();
    //}
}
