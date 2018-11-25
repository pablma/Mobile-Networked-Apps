using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour {

    string LoginURL = "http://localhost/login.php";

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

    IEnumerator LogintoDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);

        WWW www = new WWW(LoginURL, form);

        yield return www;
        infoText.text = www.text;
    }

    public void login()
    {
        StartCoroutine(LogintoDB(inputUsername.text, inputPassword.text));
    }
}
