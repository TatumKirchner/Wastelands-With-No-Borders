using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int repairAmount = 100;
    private PlayerHealth playerHealth;
    private PickupGrapple pickupGrapple;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        pickupGrapple = FindObjectOfType<PickupGrapple>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //When the player enters the trigger add health, stop the grapple and turn the pickup off.
        if (other.CompareTag("Player"))
        {
            playerHealth.AddHealth(repairAmount);
            pickupGrapple.StopGrapple();
            gameObject.SetActive(false);
        }
    }
}
