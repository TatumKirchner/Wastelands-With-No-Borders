using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTrigger : MonoBehaviour
{
    [SerializeField] private GameObject wormCart;
    [SerializeField] private GameObject wormTrack;
    [SerializeField] private GameObject wormEffects;

    [SerializeField] private bool activateWorm = false;
    [SerializeField] private bool deactivateWorm = false;
    private bool startWorm = false;
    private bool stopWorm = false;

    private void OnTriggerEnter(Collider other)
    {
        //When the player enters the trigger either turn the worm on or off.
        if (other.CompareTag("Player"))
        {
            if (activateWorm)
            {
                startWorm = true;
                stopWorm = false;
                ToggleWorm();
            }
            if (deactivateWorm)
            {
                stopWorm = true;
                startWorm = false;
                ToggleWorm();
            }
        }
    }

    void ToggleWorm()
    {
        if (startWorm)
        {
            wormCart.SetActive(true);
            wormTrack.SetActive(true);
            wormEffects.SetActive(true);
        }
        if (stopWorm)
        {
            wormCart.SetActive(false);
            wormTrack.SetActive(false);
            wormEffects.SetActive(false);
        }
    }
}
