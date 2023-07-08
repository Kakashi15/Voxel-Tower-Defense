using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosionPrefab;       // Prefab of the explosion particle effect

    private void OnCollisionEnter(Collision collision)
    {
        // Instantiate the explosion particle effect at the collision point
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Destroy the capsule and the collided object
        //Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
