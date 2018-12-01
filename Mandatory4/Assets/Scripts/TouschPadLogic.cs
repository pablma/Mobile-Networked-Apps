using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouschPadLogic : TouchFather
{

/*/
* Script by Devin Curry
* www.Devination.com
* www.youtube.com/user/curryboy001
* Please like and subscribe if you found my tutorials helpful :D
* Google+ Community: https://plus.google.com/communities/108584850180626452949
* Twitter: https://twitter.com/Devination3D
* Facebook Page: https://www.facebook.com/unity3Dtutorialsbydevin
/*/

 
    public float rotateSpeed = 100.0f;
    public int invertPitch = 1;
    public Transform player;
    private float pitch = 0.0f,
    yaw = 0.0f;
    //cache initial rotation of player so pitch and yaw don't reset to 0 before rotating
    private Vector3 oRotation;

    void Start()
    {
        //cache original rotation of player so pitch and yaw don't reset to 0 before rotating
        oRotation = player.eulerAngles;
        pitch = oRotation.x;
        yaw = oRotation.y;
    }

    public override void OnTouchBegan()
    {
        //need to cache the touch index that started on the pad so others wont interfere
        touch2Watch = TouchFather.currTouch;
    }
    public override void OnTouchMoved()
    {
        pitch -= Input.GetTouch(touch2Watch).deltaPosition.y * rotateSpeed * invertPitch * Time.deltaTime;
        yaw += Input.GetTouch(touch2Watch).deltaPosition.x * rotateSpeed * invertPitch * Time.deltaTime;
        //limit so we dont do backflips
        pitch = Mathf.Clamp(pitch, -80, 80);
        //do the rotations of our camera
        player.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    public override void OnTouchEndedAnywhere()
    {
        //the || condition is a failsafe
        if (TouchFather.currTouch == touch2Watch || Input.touches.Length <= 0)
            touch2Watch = 64;
    }
}

