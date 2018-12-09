using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerColor : NetworkBehaviour {


    [SyncVar]
    public Color pColor; // has the information and allow us to acces to the color of the mesh of all the elements and objets that make the player object

    MeshRenderer[] playerMeshRenderers;
	// Use this for initialization
	void Start () {
        playerMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i<playerMeshRenderers.Length; i++)
        {
            playerMeshRenderers[i].material.color = pColor;//changes all the elements that make the player to the color selected on the lobby menu
        }
	}
}
