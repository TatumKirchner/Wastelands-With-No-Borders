using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphadilloEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustParticles;
    [SerializeField] private float playerImpactForce = 1000;
    [SerializeField] private int damage = 30;

    private void OnCollisionEnter(Collision collision)
    {
        //When the enemy collides with the terrain layer spawn a particle system.
        if (collision.gameObject.layer == 12)
        {
            Instantiate(dustParticles, transform.position, Quaternion.identity);
        }

        //If the enemy hits the player add force at the contact point and apply damage to the player.
        if (collision.collider.CompareTag("Player"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 pos = contact.point;
            collision.rigidbody.AddForce((pos - contact.normal) * playerImpactForce);
            collision.collider.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
