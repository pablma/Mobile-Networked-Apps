using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    GameObject loginGO;
    GameObject registerGO;

    // Use this for initialization
    void Start () {
        instance = this;
        loginGO = GameObject.Find("LoginObject");
        registerGO = GameObject.Find("RegisterObject");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void goToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void goToRegister()
    {
        SceneManager.LoadScene("Register");
    }

    public void Login()
    {
        loginGO.GetComponent<Login>().login();
    }

    public void Register()
    {
        registerGO.GetComponent<DataInserter>().inserData();
    }
}
