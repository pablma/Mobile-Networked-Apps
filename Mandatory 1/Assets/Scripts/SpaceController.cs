using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceController : MonoBehaviour {

	public int spaceId = 0;
	public Sprite[] spaceSprites;

    public int spacePosition = -1; 

	public void SetSpace(int newSpaceId) {
	//Changes to test github
        //if (newSpaceId < 0) spaceId = spaceSprites.Length - 1; // Last sprite
        //else if (newSpaceId >= spaceSprites.Length) spaceId = 0; // First sprite
       /* else*/ spaceId = newSpaceId;
        GetComponent<SpriteRenderer>().sprite = spaceSprites[spaceId];
	}

	void OnMouseDown() {
		Debug.Log("ON MOUSE DOWN");
		if (GameManager.instance.yourTurn && !GameManager.instance.markedSpace && spaceId == 0 
            && !GameManager.instance.player1VictoryState && !GameManager.instance.player2VictoryState)
        {
            //here we must check the id of the player who have the turn
            if (GameManager.instance.pid == GameManager.instance.player1id)
            {
                SetSpace(1);
            }
            else if (GameManager.instance.pid == GameManager.instance.player2id)
                SetSpace(2);
            GameManager.instance.markedSpace = true;
        }
	}
}
