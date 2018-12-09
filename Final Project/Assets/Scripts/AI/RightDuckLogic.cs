using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RightDuckLogic : NetworkBehaviour{

    // Variable to edit the velocity
    public float speed = 10f;
    private Rigidbody rb;

    // The duck must move when we activate it
    private void OnEnable()
    {
        // We access the rigidbody component and add velocity to the object
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.right * speed;
    }

    //Unspawn the object when collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        Pool.instance.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }
}
