using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataLoader : MonoBehaviour {

    public string[] users;

    // Use this for initialization
    IEnumerator Start () {
		WWW userData = new WWW("http://localhost/Mandatory3_Database/database.php");
        yield return userData;
        string userDataString = userData.text;
        Debug.Log(userDataString);
        users = userDataString.Split(';');
        Debug.Log(getDataValue(users[0], "Username:"));
	}

    string getDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);

        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));

        return value;
    }
}
