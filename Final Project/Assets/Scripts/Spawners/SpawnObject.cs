using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    public GameObject ObjectToSpawn;
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
            Instantiate(ObjectToSpawn);
            timer = TimeToRespawn;
        }
	}
}
