using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LobbyController : MonoBehaviour {

	// Reference to buttons with name of players in lobby:
	public Button[] playersButton;
	private int maxPlayersInLobby = 0;
	private int[] playersPid;
	private string[] playersName;

	// Reference to debug Text:
	public Text debugText;
	public bool showDebug = true;

	public string lobbyUpdateUrl = "https://mediastudent.no/SPO3020/lobby_v5/lobbyupdate.php";
	public string gameStartUrl = "https://mediastudent.no/SPO3020/lobby_v5/gamestart.php";
	public string gameSetDataUrl = "https://mediastudent.no/SPO3020/lobby_v5/gamesetdata.php";

	void Start () {
		// Set max number of players (buttons available) in lobby:
		maxPlayersInLobby = playersButton.Length;

		// Initialize array with player pids and names in lobby:
		playersPid = new int[maxPlayersInLobby];
		playersName = new string[maxPlayersInLobby];

		// Hide all buttons:
		for (int i=0; i<playersButton.Length; i++) {
			playersButton[i].gameObject.SetActive(false);
		}

		// Debug Text:
		if (!showDebug) debugText.gameObject.SetActive(false);
		else debugText.gameObject.SetActive(true);
		debugText.text = "NICKNAME: " + GameManager.instance.nickname;

		// Start first server pull:
		PullLobby();
	}
	
	public void PullLobby() {
		StartCoroutine(PullLobbyServer());
	}

	IEnumerator PullLobbyServer() {
		Debug.Log("Load from: " + lobbyUpdateUrl + "?pid=" + GameManager.instance.pid + "&pullid=" + GameManager.instance.pullid);

		UnityWebRequest www = UnityWebRequest.Get(lobbyUpdateUrl + "?pid=" + GameManager.instance.pid + "&pullid=" + GameManager.instance.pullid);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
			// Show error:
			debugText.text = "NETWORK ERROR:\n" + www.error;			
		} else {
			debugText.text = "RESPONSE:\n" + www.downloadHandler.text;

			// Parse JSON data from server:
			LobbyUpdateResponse responseJson = JsonUtility.FromJson<LobbyUpdateResponse>(www.downloadHandler.text);

			// Handle different "result" values:
			if (responseJson.result == "INLOBBY") {
				debugText.text = "INLOBBY:\n" + www.downloadHandler.text;
				// Set up lobby player buttons:
				for (int i=0; i<responseJson.num_players; i++) {
					// Show and enable (clickable):
					playersButton[i].gameObject.SetActive(true);
					playersButton[i].enabled = true;
					// Set player name as text:
					playersButton[i].GetComponentInChildren<Text>().text = responseJson.names[i];
					// Set player id and name in arrays:
					playersPid[i] = responseJson.pids[i];
					playersName[i] = responseJson.names[i];
				}
				for (int i=responseJson.num_players; i<maxPlayersInLobby; i++) {
					// Hide unused button:
					playersButton[i].gameObject.SetActive(false);
				}

			} else if (responseJson.result == "INGAME") {
				debugText.text = "INGAME:\n" + www.downloadHandler.text;
				// Cancel invoked lobby pull:
				CancelInvoke("PullLobby");
				// Cancel any ongoing web requests:
				// NOT IMPLEMENTED!
				GameManager.instance.gid = responseJson.gid;
				SceneManager.LoadScene("Game");

			} else if (responseJson.result == "PIDUNKNOWN") {
				debugText.text = "PIDUNKNOWN!";
				// Cancel invoked lobby pull:
				CancelInvoke("PullLobby");				
				// Cancel any ongoing web requests:
				// NOT IMPLEMENTED!
				GameManager.instance.pid = -1;
				SceneManager.LoadScene("Register");

			} else if (responseJson.result == "SQLERROR") {
				debugText.text = "SQLERROR:\n" + responseJson.info;

			} else if (responseJson.result == "PIDNOTSET") {
				debugText.text = "PIDNOTSET!";
			}
		}

		// Next pull in "interval" seconds:
		Invoke("PullLobby", GameManager.instance.pullIntervalLobby);
		GameManager.instance.pullid++;
	}

	IEnumerator GameStartServer() {
		Debug.Log("Upload to: " + gameStartUrl);

		// POST data to send server:
		List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
		formData.Add(new MultipartFormDataSection("pid", GameManager.instance.pid.ToString()));
		formData.Add(new MultipartFormDataSection("opponentid", GameManager.instance.player2id.ToString()));

		// Send REQUEST:
		UnityWebRequest www = UnityWebRequest.Post(gameStartUrl, formData);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
			// Show error:
			debugText.text = "NETWORK ERROR:\n" + www.error;
			// Start pulling lobby again:
			PullLobby();
		} else {
			Debug.Log("Form upload complete!");
			debugText.text = "RESPONSE:\n" + www.downloadHandler.text;

			// Parse JSON data from server:
			GameStartResponse responseJson = JsonUtility.FromJson<GameStartResponse>(www.downloadHandler.text);

			// If successfully:
			if (responseJson.result == "OK") {
				debugText.text = "OK:\n" + www.downloadHandler.text;
				// Set values in GameManager:
				GameManager.instance.gid = responseJson.gid;
				GameManager.instance.yourTurn = true;

				// Set shared data object on server:
				StartCoroutine(GameSetDataServer());

			} else if (responseJson.result == "ERROR") {
				// Error handling...
				debugText.text = "ERROR:\n" + responseJson.info;

				// Start pulling lobby again:
				PullLobby();
			}
		}
	}

	IEnumerator GameSetDataServer() {
		Debug.Log("Upload to: " + gameSetDataUrl);

		// Shared data on server as JSON:
		GameSharedData dataObj = new GameSharedData();
		dataObj.player1id = GameManager.instance.player1id;
		dataObj.player2id = GameManager.instance.player2id;
		dataObj.player1name = GameManager.instance.player1name;
		dataObj.player2name = GameManager.instance.player2name;
		dataObj.playerturn = 0;
		dataObj.boardarray = "0,0,0,0,0,0,0,0,0";
		string dataJson = JsonUtility.ToJson(dataObj);

		// POST data to send server:
		List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
		formData.Add(new MultipartFormDataSection("pid", GameManager.instance.pid.ToString()));
		formData.Add(new MultipartFormDataSection("gid", GameManager.instance.gid.ToString()));
		formData.Add(new MultipartFormDataSection("data", dataJson));

		// Send REQUEST:
		UnityWebRequest www = UnityWebRequest.Post(gameSetDataUrl, formData);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
			// Show error:
			debugText.text = "NETWORK ERROR:\n" + www.error;
			// Start pulling lobby again???
		} else {
			Debug.Log("Form upload complete!");
			debugText.text = "RESPONSE:\n" + www.downloadHandler.text;

			// Parse JSON data from server:
			GameSetDataResponse responseJson = JsonUtility.FromJson<GameSetDataResponse>(www.downloadHandler.text);

			// If successfully:
			if (responseJson.result == "OK") {
				debugText.text = "OK:\n" + www.downloadHandler.text;
				// Open Game scene:
				SceneManager.LoadScene("Game");

			} else if (responseJson.result == "ERROR") {
				// Error handling...
				debugText.text = "ERROR:\n" + responseJson.info;

				// Start pulling lobby again:
				PullLobby();
			}
		}
	}

	public void PlayerSelectedButtonHandler(int buttonId) {
		Debug.Log("BUTTON CLICKED: " + buttonId + ", opponentid: " + playersPid[buttonId] + ", name: "  + playersName[buttonId]);
		debugText.text = "BUTTON CLICKED: " + buttonId + ", opponentid: " + playersPid[buttonId] + ", name: "  + playersName[buttonId];
		
		// Cancel invoked lobby pull:
		CancelInvoke("PullLobby");
		// Cancel any ongoing web requests:
		// NOT IMPLEMENTED!

		// Update game data:
		GameManager.instance.player1id = GameManager.instance.pid; // (you)
		GameManager.instance.player2id = playersPid[buttonId]; // id opponent
		GameManager.instance.player1name = GameManager.instance.nickname; // (you)
		GameManager.instance.player2name = playersName[buttonId]; // name opponent

		// Start new game on the server:
		StartCoroutine(GameStartServer());
	}

}
