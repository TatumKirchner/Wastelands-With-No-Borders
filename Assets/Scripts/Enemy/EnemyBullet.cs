using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float impactForce = 1000;

    private void OnCollisionEnter(Collision collision)
    {
        //When the bullet hits something look for a rigidbody.
        //If it finds one get add force to it
        if (collision.collider.attachedRigidbody != null)
        {
            collision.collider.attachedRigidbody.AddForce((transform.position - collision.transform.position).normalized * impactForce);
        }

        Destroy(gameObject);
    }
}
