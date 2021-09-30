using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    [SerializeField] private float explosionForce = 5000f;
    [SerializeField] private float upwardsLift = 2500f;
    [SerializeField] private float explosionRadius = 15f;
    private EnemyHealth EnemyHealth;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the projectile hits something with a rigidbody add explosion force.
        if (collision.rigidbody != null)
        {
            collision.collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsLift);
            EnemyHealth = collision.collider.GetComponent<EnemyHealth>();
            //If the collision was with an enemy do damage.
            if (collision.collider.CompareTag("Enemy"))
            {
                EnemyHealth.TakeDamage(100);
            }
            Destroy(gameObject);
        }
    }
}
