using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    // When something collides with the DeadZone
    private void OnCollisionEnter(Collision collision)
    {
        ObjectPooler.instance.killGameObject(collision.gameObject);
    }
}
