using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class GameController : MonoBehaviour {

	// Reference to board:
	public GameObject[] spaces;
	private int[]  board;

	// Reference to title Text:
	public Text titleText;

	// Reference to save board Button:
	public Button saveBoardButton;

	private string lastUpdated = "1971-01-01 08:00:00";

	// Reference to debug Text:
	public Text debugText;
	public bool showDebug = true;

	public string gameUpdateUrl = "https://mediastudent.no/SPO3020/lobby_v5/gameupdate.php";
	public string gameSetDataUrl = "https://mediastudent.no/SPO3020/lobby_v5/gamesetdata.php";
	public string gameEndUrl = "https://mediastudent.no/SPO3020/lobby_v5/gameend.php";

	private string[] tempStringArray;

	void Start () {
		// Initialize board array:
		board = new int[spaces.Length];
		for (int i=0; i<board.Length; i++) {
			board[i] = spaces[i].GetComponent<SpaceController>().spaceId;
		}

		// Debug Text:
		if (!showDebug) debugText.gameObject.SetActive(false);
		else debugText.gameObject.SetActive(true);
		debugText.text = "NICKNAME: " + GameManager.instance.nickname;

		// Update all elements:
		UpdateScene();

		// Start first server pull:
		PullGame();
	}

	public void PullGame() {
		StartCoroutine(PullGameServer());
	}

	IEnumerator PullGameServer() {
		Debug.Log("Load from: " + gameUpdateUrl + "?pid=" + GameManager.instance.pid + "&gid=" + GameManager.instance.gid + "&lastupdated=" + UnityWebRequest.EscapeURL(lastUpdated) + "&pullid=" + GameManager.instance.pullid);

		UnityWebRequest www = UnityWebRequest.Get(gameUpdateUrl + "?pid=" + GameManager.instance.pid + "&gid=" + GameManager.instance.gid + "&lastupdated=" + UnityWebRequest.EscapeURL(lastUpdated) + "&pullid=" + GameManager.instance.pullid);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
			// Show error:
			debugText.text = "NETWORK ERROR:\n" + www.error;			
		} else {
			debugText.text = "RESPONSE:\n" + www.downloadHandler.text;

			// Parse JSON data from server:
			GameSetUpdateResponse responseJson = JsonUtility.FromJson<GameSetUpdateResponse>(www.downloadHandler.text);

			// Handle different "result" values:
			if (responseJson.result == "UPDATE") {
				debugText.text = "UPDATE:\n" + www.downloadHandler.text;
				
				// Parse data string as JSON:
				GameSharedData dataObj = JsonUtility.FromJson<GameSharedData>(responseJson.data);

				// Update general data:
				GameManager.instance.player1id = dataObj.player1id;
				GameManager.instance.player2id = dataObj.player2id;
				GameManager.instance.player1name = dataObj.player1name;
				GameManager.instance.player2name = dataObj.player2name;
				
				// Update turn:
				GameManager.instance.yourTurn = false;
				if (dataObj.playerturn == 0 && GameManager.instance.player1id == GameManager.instance.pid) GameManager.instance.yourTurn = true;
				else if (dataObj.playerturn == 1 && GameManager.instance.player2id == GameManager.instance.pid) GameManager.instance.yourTurn = true;

				// Update board:
				tempStringArray = dataObj.boardarray.Split(',');
				for (int i=0; i<tempStringArray.Length; i++) {
					if (i<board.Length) board[i] = System.Convert.ToInt32(tempStringArray[i]);
				}
				
				// Update last updated:
				lastUpdated = responseJson.lastupdated;

				// Update all elements:
				UpdateScene();
			} else if (responseJson.result == "ENDGAME") {
				debugText.text = "ENDGAME!";
				// Cancel invoked game pull:
				CancelInvoke("PullGame");
				// Cancel any ongoing web requests:
				// NOT IMPLEMENTED!

				// Update game data:
				GameManager.instance.player1id = -1;
				GameManager.instance.player2id = -1;
				GameManager.instance.player1name = "";
				GameManager.instance.player2name = "";
				GameManager.instance.gid = -1;

				// Back to lobby:
				SceneManager.LoadScene("Lobby");

			} else if (responseJson.result == "ERROR") {
				debugText.text = "ERROR:\n" + responseJson.info;

			} else if (responseJson.result == "SQLERROR") {
				debugText.text = "SQLERROR!";

			} else if (responseJson.result == "OK") {
				debugText.text = "OK:\n" + www.downloadHandler.text;
				// No new data, do nothing...
			}
		}

		// Next pull in "interval" seconds:
		Invoke("PullGame", GameManager.instance.pullIntervalGame);
		GameManager.instance.pullid++;
	}

	IEnumerator GameSetDataServer() {
		Debug.Log("Upload to: " + gameSetDataUrl);

		// Create array string from board:
		string boardArrayString = "";
		for (int i=0; i<board.Length; i++) {
			if (boardArrayString != "") boardArrayString += ",";
			boardArrayString += board[i];
		}

		// Shared data on server as JSON:
		GameSharedData dataObj = new GameSharedData();
		dataObj.player1id = GameManager.instance.player1id;
		dataObj.player2id = GameManager.instance.player2id;
		dataObj.player1name = GameManager.instance.player1name;
		dataObj.player2name = GameManager.instance.player2name;
		// Set opponent as turn (0 or 1):
		dataObj.playerturn = 0;
		if (GameManager.instance.player1id == GameManager.instance.pid) dataObj.playerturn = 1;
		dataObj.boardarray = boardArrayString;
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
		} else {
			Debug.Log("Form upload complete!");
			debugText.text = "RESPONSE:\n" + www.downloadHandler.text;

			// Parse JSON data from server:
			GameSetDataResponse responseJson = JsonUtility.FromJson<GameSetDataResponse>(www.downloadHandler.text);

			// If successfully:
			if (responseJson.result == "OK") {
				debugText.text = "OK:\n" + www.downloadHandler.text;
				// Do nothing...

			} else if (responseJson.result == "ERROR") {
				// Error handling...
				debugText.text = "ERROR:\n" + responseJson.info;
			}
		}
	}

	IEnumerator GameEndServer() {
		Debug.Log("Load from: " + gameEndUrl + "?pid=" + GameManager.instance.pid + "&gid=" + GameManager.instance.gid);

		UnityWebRequest www = UnityWebRequest.Get(gameEndUrl + "?pid=" + GameManager.instance.pid + "&gid=" + GameManager.instance.gid);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
			// Show error:
			debugText.text = "NETWORK ERROR:\n" + www.error;			
		} else {
			debugText.text = "RESPONSE:\n" + www.downloadHandler.text;

			// Parse JSON data from server:
			GameEndResponse responseJson = JsonUtility.FromJson<GameEndResponse>(www.downloadHandler.text);

			// Handle different "result" values:
			if (responseJson.result == "OK") {
				debugText.text = "OK:\n" + www.downloadHandler.text;
				
				// Update game data:
				GameManager.instance.player1id = -1;
				GameManager.instance.player2id = -1;
				GameManager.instance.player1name = "";
				GameManager.instance.player2name = "";
				GameManager.instance.gid = -1;
				
				// Back to lobby:
				SceneManager.LoadScene("Lobby");

			} else if (responseJson.result == "ERROR") {
				debugText.text = "ERROR:\n" + responseJson.info;
			}
		}
	}

	public void SaveBoardButtonHandler() {
		//Debug.Log("SAVE BOARD BUTTON CLICKED!");
		debugText.text = "SAVE BOARD BUTTON CLICKED!";

        // Update game data:
        if (GameManager.instance.markedSpace)
        {
            debugText.text = "SAVE BOARD BUTTON CLICKED!";
            GameManager.instance.yourTurn = false;
            GameManager.instance.markedSpace = false;
            // Update board array:
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = spaces[i].GetComponent<SpaceController>().spaceId;
            }

            //if (checkVictory())
            //{
            //    if (GameManager.instance.pid == GameManager.instance.player1id)
            //        GameManager.instance.player1VictoryState = true;
            //    else if(GameManager.instance.pid == GameManager.instance.player2id)
            //        GameManager.instance.player2VictoryState = true;
                UpdateScene();
                //winstate();
                // Update shared data object on server:
                StartCoroutine(GameSetDataServer());
            //}
            //else
            //{
            //    // Update all elements:
            //    UpdateScene();
            //    // Update shared data object on server:
            //    StartCoroutine(GameSetDataServer());
            //}
        }
        else
            debugText.text = "NO SPACE SELECTED";
    }

	public void EndGameButtonHandler() {
		Debug.Log("END GAME BUTTON CLICKED!");
		debugText.text = "END GAME BUTTON CLICKED!";

		// Cancel invoked lobby pull:
		CancelInvoke("PullLobby");
		// Cancel any ongoing web requests:
		// NOT IMPLEMENTED!

		// End game on server:
		StartCoroutine(GameEndServer());
	}

	void UpdateScene() {
		// Set title (Player1 vs. player2):
		titleText.text = GameManager.instance.player1name + " vs. " + GameManager.instance.player2name;


        // Set all sprites on board:
        for (int i=0; i<board.Length; i++) {
			spaces[i].GetComponent<SpaceController>().SetSpace(board[i]);
		}

        if (checkVictory())
        {
            if (GameManager.instance.pid == GameManager.instance.player1id)
                GameManager.instance.player1VictoryState = true;
            else if (GameManager.instance.pid == GameManager.instance.player2id)
                GameManager.instance.player2VictoryState = true;
        }

        // Update Save Board Button:
        if (GameManager.instance.player1VictoryState || GameManager.instance.player2VictoryState)
        {
            if (GameManager.instance.pid == GameManager.instance.player1id && GameManager.instance.player1VictoryState)
                titleText.text = "GANASTE";
            else if (GameManager.instance.pid == GameManager.instance.player2id && GameManager.instance.player2VictoryState)
                titleText.text = "GANASTE";
            else titleText.text = "PERDISTE";

            saveBoardButton.gameObject.SetActive(false);
        }
        else
        {
            if (GameManager.instance.yourTurn) saveBoardButton.gameObject.SetActive(true);
            else saveBoardButton.gameObject.SetActive(false);
        }
		
	}

    
    bool checkVictory()
    {
        int dim = 3;
        bool victory = false;

        int i = 0;
        int j = 0;

        //horizontal
        while (!victory && i < dim)
        {
            j = i * dim;
            if (board[j] != 0 && board[j] == board[j + 1] && board[j + 1] == board[j + 2])
                victory = true;
            i++;
        }

        //vertical
        i = 0;
        while (!victory && i < dim)
        {
            if (board[i] != 0 && board[i] == board[i + dim] && board[i + dim] == board[i + 2*dim])
                victory = true;
            i++;
        }

        //Descendent diagonal from top left corner
        if (board[0] != 0 && board[0] == board[dim +1] && board[dim +1] == board[2 * (dim+1)])
            victory = true;

        //Descendent diagonal from top right corner
        if (board[dim - 1] != 0 && board[dim-1] == board[2*(dim - 1)] && board[2 * (dim - 1)] == board[3 * (dim - 1)])
            victory = true;

        return victory;
    }

    void winstate()
    {
        // Set title (Player1 vs. player2):
        if (GameManager.instance.pid == GameManager.instance.player1id && GameManager.instance.player1VictoryState)
            titleText.text = "GANASTE";
        else if (GameManager.instance.pid == GameManager.instance.player2id && GameManager.instance.player2VictoryState)
            titleText.text = "GANASTE";
        else titleText.text = "PERDISTE";
        //titleText.text = GameManager.instance.player1name + " vs. " + GameManager.instance.player2name;

        // Set all sprites on board:
        for (int i = 0; i < board.Length; i++)
        {
            spaces[i].GetComponent<SpaceController>().SetSpace(board[i]);
        }

        // Update Save Board Button:
        saveBoardButton.gameObject.SetActive(false);
    }
}
