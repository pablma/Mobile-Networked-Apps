using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftDuckLogic : MonoBehaviour {

    // Variable to edit the velocity
    public float speed = 10f;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.left * speed;
    }
}
