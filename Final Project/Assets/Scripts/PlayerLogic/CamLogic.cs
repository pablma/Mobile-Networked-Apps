using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CamLogic : NetworkBehaviour
{
    public float speed = 3.5f;
    private float X;
    private float Y;

    void Update()
    {//camera logic for mouse, it allow us to move the camera with the mouse to allow playing the game on computers
        if (!isLocalPlayer)
        {
            Camera vari;
            vari = gameObject.GetComponent<Camera>();
            vari.enabled = false;

            return;
        }

        if (Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        }
    }
}
