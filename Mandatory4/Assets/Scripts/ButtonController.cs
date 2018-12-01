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

        ////////if (!isServer)
        ////////{
        ////////    if (!isLocalPlayer)
        ////////    {
        ////////        //AttackOtherUnit(gameObject);
        ////////    }
        ////////    else CmdFire();
        ////////}
        ////////else CmdFire();
        //////////////////////if (isServer)
        //////////////////////    RpcFire();
        //////////////////////else CmdFire();


        //if (!isLocalPlayer)
        //{

        //}
        //else
        //{
        //    var health = gameObject.GetComponent<Health>();
        //    if (health != null)
        //        health.TakeDamage(50);
        //}
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
    //[Command]
    //private void CmdFire()
    //{
    //    var health = gameObject.GetComponent<Health>();
    //    if (health != null)
    //        health.TakeDamage(50);
    //    RpcFire();
    //}

    //[ClientRpc]
    //void RpcFire()
    //{
    //    var health = gameObject.GetComponent<Health>();
    //    if (health != null)
    //        health.TakeDamage(50);
    //}

    private void Fire()
    {
        var health = gameObject.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(10);
    }

    //[Client]
    //public void AttackOtherUnit(GameObject o)
    //{
    //    // have us attack the other unit on the server!
    //    NetworkIdentity ident = o.GetComponent<NetworkIdentity>();
    //    CmdAttackUnit(ident);
    //}

    //[Command]
    //public void CmdAttackUnit(NetworkIdentity otherUnit)
    //{
    //    // this method is called on the server, so long as we call the method from the proper client
    //    // if called by a client without authority, it will simply return immediately and do nothing!
    //    Health comp = otherUnit.GetComponent<Health>();
    //    otherUnit.GetComponent<SpriteRenderer>().color = Color.green;
    //    comp.TakeDamage(50);
    //}
}
