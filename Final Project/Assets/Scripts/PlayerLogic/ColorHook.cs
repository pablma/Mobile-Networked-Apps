using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorHook: Prototype.NetworkLobby.LobbyHook
{


    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);

        Prototype.NetworkLobby.LobbyPlayer lobbyP = lobbyPlayer.GetComponent<Prototype.NetworkLobby.LobbyPlayer>();
        PlayerColor cameraPlayer = gamePlayer.GetComponent<PlayerColor>();

        cameraPlayer.pColor = lobbyP.playerColor;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
