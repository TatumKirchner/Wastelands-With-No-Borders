using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDeath : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    //Start coroutine to kill the player.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(KillPlayer());
        }
    }

    //Set player health to 0 then wait a beat to let the particle system to play. Then call take damage to kill the player.
    IEnumerator KillPlayer()
    {
        playerHealth.currentHealth = 0;
        yield return new WaitForSeconds(.25f);
        playerHealth.TakeDamage(100);
    }
}
