using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    [Tooltip("Write the same tag the GameObject has in the pooler")]
    public string tag;

    ObjectPooler objectPooler;
    public float TimeToRespawn = 2f;
    private float timer;


	// Use this for initialization
	void Start () {
        objectPooler = ObjectPooler.instance;
        timer = TimeToRespawn;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            objectPooler.spawnFromPool(tag, transform.position, transform.rotation);
            timer = TimeToRespawn;
        }
	}
}
