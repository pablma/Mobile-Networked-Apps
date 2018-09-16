using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour {

    public GameObject[] spaces;
    public int lastMarked;
    // Use this for initialization
    void Start () {
        for (int i = 0; i < spaces.Length; i++)
        {
            spaces[i].GetComponent<SpaceController>().spacePosition = i;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void resetSpace()
    {
        spaces[lastMarked].GetComponent<SpaceController>().SetSpace(0);
    }

    public void markSpace(int spaceNumber)
    {
        lastMarked = spaceNumber;
    }
}
