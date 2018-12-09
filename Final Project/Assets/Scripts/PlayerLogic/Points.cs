using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Points : NetworkBehaviour
{

    //This script is the one that has the logic of the points that each player has
    FingerGun fingerGun;

    public const int initialPoints = 0;

    [SyncVar(hook = "OnChangePoints")]//each time this variable changes call the function OnChangePonts for all the player and clients
    public int currentPoints = initialPoints;

    int playerId;

    // Use this for initialization
    void Start () {
        //getting necessary parameters as the player id of the one that have this script
        fingerGun = GetComponent<FingerGun>();
        playerId = fingerGun.getPlayerId();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnChangePoints(int currentPoints)//will be called each time the variable point changes, it will update the gameManagers each time, a puntuation changes
    {
        if(fingerGun.getPlayerId() == 0)//we must know wich player is the one that is increasing his points in order to update correctly the Gm
        {
            GameManager.instance.updatePlayerOnePoints(currentPoints);

        }
        else
        {
            GameManager.instance.updatePlayerTwoPoints(currentPoints);

        }
    }

    public int GetPlayerId()//gets the player Id of the owner object of the script 
    {
        return playerId;
    }

    public void giveMePoints(int points)// five points to the counter of the owner player. Will be called from other script (Duckpoints)
    {
        currentPoints += points;
    }
}
