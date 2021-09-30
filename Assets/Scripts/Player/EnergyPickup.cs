using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    [SerializeField] private float pickupAmount = 60;
    private PickupGrapple pickupGrapple;
    private GameObject player;
    private KETweapons KETweapons;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        KETweapons = player.GetComponent<KETweapons>();
        pickupGrapple = player.GetComponent<PickupGrapple>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //When the player enters the trigger add energy to the player, stop the grapple and destroy the pickup.
        if (other.CompareTag("PlayerCollider"))
        {
            KETweapons.AddEnergy(pickupAmount);
            pickupGrapple.StopGrapple();
            Destroy(gameObject);
        }
    }
}
