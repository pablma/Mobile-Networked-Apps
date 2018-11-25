using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataInserter : MonoBehaviour {

    string createUserURL = "http://localhost/insertUser.php";

    public InputField inputUsername;
    public InputField inputPassword;
    public Text infoText;

    // Use this for initialization
    void Start () {
        inputPassword.contentType = InputField.ContentType.Password;
	}
	
	// Update is called once per frame
	void Update () {
  
	}

    IEnumerator createUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);

        WWW www = new WWW(createUserURL, form);
        yield return www;
        infoText.text = www.text;
        Debug.Log(www.text);
    }

    public void inserData()
    {
        StartCoroutine(createUser(inputUsername.text, inputPassword.text));
    }
}
