using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerColor : NetworkBehaviour {


    [SyncVar]
    public Color pColor;

    MeshRenderer[] playerMeshRenderers;
	// Use this for initialization
	void Start () {
        playerMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i<playerMeshRenderers.Length; i++)
        {
            playerMeshRenderers[i].material.color = pColor;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
