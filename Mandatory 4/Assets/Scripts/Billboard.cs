using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    private Camera camera;
    //allow us to face the camera
    void Start()
    {
        if ((GameObject.Find("Main Camera") != null) && (GameObject.Find("Main Camera").GetComponent<Camera>() != null))
        {
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }

    void FixedUpdate()
    {
        if (camera != null)
        {
            transform.LookAt(camera.transform);
        }
    }
}
