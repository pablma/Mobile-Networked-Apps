using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TextureLoader : MonoBehaviour {

	public InputField textureUrlInput;
	public string textureLoadUrl = "https://mediastudent.no/SPO3020/platform_gfx/tileset.png";
	public GameObject spritePrefab;
	
	private GameObject spriteObj;

	// Use this for initialization
	void Start () {
		// Set texture URL in input:
		textureUrlInput.text = textureLoadUrl;
		// Instantiate gameobject with sprite renderer:
		spriteObj = Instantiate(spritePrefab, new Vector3(0f, -1f, 0f), Quaternion.identity);
	}
	
	IEnumerator GetTexture() {
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(textureUrlInput.text);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		} else {
			Debug.Log("Texture loaded!");
			// Get texture data:
			Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			// Create new sprite based on loaded texture:
			Sprite mySprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
			// Attach new sprite on instantiated gameobjects sprite renderer:
			spriteObj.GetComponent<SpriteRenderer>().sprite = mySprite;
		}
	}

	public void LoadButtonHandler() {
		StartCoroutine(GetTexture());
	}
}
