using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormDamage : MonoBehaviour
{
    [SerializeField] int damage = 120;
    [SerializeField] float impactForce = 2000;
    PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //When a collision with the player happens add force to the player at the collision point, and apply damage to the player.
        if (collision.collider.CompareTag("Player"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 pos = contact.point;
            collision.rigidbody.AddForce((pos - contact.normal) * impactForce);
            playerHealth.TakeDamage(damage);
        }
    }
}
