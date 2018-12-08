using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RightDuckLogic : NetworkBehaviour{

    // Variable to edit the velocity
    public float speed = 10f;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        
    }

    // The duck must move when we activate it
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.right * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Pool.instance.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }
}
