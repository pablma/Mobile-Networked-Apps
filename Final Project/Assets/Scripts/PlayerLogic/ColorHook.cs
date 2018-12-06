using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorHook: Prototype.NetworkLobby.LobbyHook
{
    //this component allow us to keep the color selected by the player on the lobby, it will be the color of the player when the game start.

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);

        Prototype.NetworkLobby.LobbyPlayer lobbyP = lobbyPlayer.GetComponent<Prototype.NetworkLobby.LobbyPlayer>();
        PlayerColor cameraPlayer = gamePlayer.GetComponent<PlayerColor>(); //access to the playerColor component, wich has the information of the color

        cameraPlayer.pColor = lobbyP.playerColor;//Makes the player color the same as the selected by the player on the lobby
    }
}
