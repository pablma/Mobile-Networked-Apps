using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        //the bullets are destroyed on collision, make damage if the collider is player one
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(10);
        Destroy(gameObject);
    }
}
