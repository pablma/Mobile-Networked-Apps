using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    [SyncVar]
    public int bulletId = 9;


    private void Update()
    {
        if(bulletId == 0)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        //Allow us to distinguish the two bullets
    }

    void OnCollisionEnter(Collision collision)
    {
        //the bullets are destroyed on collision, make damage if the collider is player one
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(10);
        Destroy(gameObject);
    }

    public void GiveName(int ID)
    {
        bulletId = ID;
    }

}
