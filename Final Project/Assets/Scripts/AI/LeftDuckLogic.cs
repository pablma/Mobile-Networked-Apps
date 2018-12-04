using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftDuckLogic : MonoBehaviour{

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
}
