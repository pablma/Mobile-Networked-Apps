using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public int pid = -1; // player id (you)
	public int gid = -1; // game id
	public string nickname = ""; // name (you)
	public int player1id = -1; // player1 id
	public int player2id = -1; // player2 id
	public string player1name = ""; // player1 name
	public string player2name = ""; // player2 name
	public bool yourTurn = false;
	public int pullid = 1; // pull counter
	public float pullIntervalLobby = 5f; // seconds
	public float pullIntervalGame = 5f; // seconds
    public bool markedSpace = false;

    public bool player1VictoryState = false;
    public bool player2VictoryState = false;

    // Generate SHA-1: http://www.sha1-online.com/
    public string appId = "812abdcf83c6b4fe37fe2f37bd9f8c0a3a5b5746"; // unique application id

	void Awake () {
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

}
