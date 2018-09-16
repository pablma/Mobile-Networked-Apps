using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RegisterController : MonoBehaviour {

	// Reference to nickname input:
	public InputField nickname;

	// Reference to debug Text:
	public Text debugText;
	public bool showDebug = true;

	public string lobbyStartUrl = "https://mediastudent.no/SPO3020/lobby_v5/lobbystart.php";

	void Start () {
		// Debug Text:
		if (!showDebug) debugText.gameObject.SetActive(false);
		else debugText.gameObject.SetActive(true);

	}

	IEnumerator EnterLobbyServer() {
		Debug.Log("Upload to: " + lobbyStartUrl);

		// POST data to send server:
		List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
		formData.Add(new MultipartFormDataSection("name", GameManager.instance.nickname));
		formData.Add(new MultipartFormDataSection("appid", GameManager.instance.appId));

		// Send REQUEST:
		UnityWebRequest www = UnityWebRequest.Post(lobbyStartUrl, formData);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
			// Show error:
			debugText.text = "NETWORK ERROR:\n" + www.error;
		} else {
			Debug.Log("Form upload complete!");
			debugText.text = "Result:\n" + www.downloadHandler.text;

			// Parse JSON data from server:
			LobbyStartResponse responseJson = JsonUtility.FromJson<LobbyStartResponse>(www.downloadHandler.text);

			// If successfully:
			if (responseJson.result == "OK") {
				// Set values in GameManager:
				GameManager.instance.pid = responseJson.pid;
				// Load Lobby scene:
				SceneManager.LoadScene("Lobby");
			} else if (responseJson.result == "ERROR") {
				// Error handling...
				debugText.text = "ERROR:\n" + responseJson.info;
			}
		}
	}
	
	public void EnterLobbyButtonHandler() {
		debugText.text = "BUTTON CLICKED: Enter Lobby";
		if (nickname.text == "") {
			Debug.Log("ERROR: No nickhame typed!");
			debugText.text += "\nERROR: No nickname typed!";
		} else {
			Debug.Log("Nickname: " + nickname.text);
			debugText.text += "\nNickname: " + nickname.text;
			debugText.text += "\nAppId: " + GameManager.instance.appId;
			GameManager.instance.nickname = nickname.text;

			// UnityWebRequest as coroutine:
			StartCoroutine(EnterLobbyServer());			
		}
	}


}
