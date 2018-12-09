using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeftDuckLogic : NetworkBehaviour
{

    // Variable to edit the velocity
    public float speed = 10f;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {

    }

    // The duck must move when we activate it
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.left * speed;
    }

    private void OnCollisionEnter(Collision collision)// unsepawn when it collides with another object
    {
        Pool.instance.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }
}
