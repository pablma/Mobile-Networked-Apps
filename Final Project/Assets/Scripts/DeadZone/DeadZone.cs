using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    ObjectPooler objectPooler;

	// Use this for initialization
	void Start () {
        objectPooler = ObjectPooler.instance;
	}

    // When something collides with the DeadZone
    private void OnCollisionEnter(Collision collision)
    {
        objectPooler.killGameObject(collision.gameObject);
    }
}
