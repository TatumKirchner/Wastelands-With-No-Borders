using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    public PlayerHealth playerHealth;
    private IsPaused pausemanager;
    private bool takingDamage = false;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        pausemanager = FindObjectOfType<IsPaused>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //When the player enters the trigger start applying damage.
        if (other.CompareTag("Player"))
        {
            takingDamage = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //When the player exits the trigger stop applying damage.
        if (other.CompareTag("Player"))
        {
            takingDamage = false;
        }
    }

    private void Update()
    {
        //Call the players take damage method
        if (takingDamage && !pausemanager.paused)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
