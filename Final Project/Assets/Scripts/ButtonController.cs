using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ButtonController : NetworkBehaviour
{

    
	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            //this is only going to work if we desable the transform networking options syncronization. Because we need the local player
            //on the same position for the local player and the client player in his running build.
            GetComponent<SpriteRenderer>().material.color = Color.red;
            transform.position = new Vector3(0.0f, -2.0f, 0.0f);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
    }


    void OnMouseDown()
    {
        if(isLocalPlayer)
            CmdFire();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        { }

            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            transform.position = new Vector3(0.0f, 2.0f, 0.0f);
    }


    [Command]
    private void CmdFire()
    {
        var health = gameObject.GetComponent<ButtonHealth>();
        if (health != null)
            health.TakeDamage(10);
    }
   

    private void Fire()
    {
        var health = gameObject.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(10);
    }
}
