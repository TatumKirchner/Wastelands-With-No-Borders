using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDustDevils : MonoBehaviour
{
    [SerializeField] private bool followTarget = false;
    private DustDevilTarget devilTarget;

    private void Start()
    {
        devilTarget = GameObject.Find("Game Manager").GetComponent<DustDevilTarget>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //When the player enters the trigger either set the global target as the player, or set it to null so the enemies stops chasing the player.
        if (followTarget)
        {
            if (other.CompareTag("Player"))
            {
                devilTarget.target = other.transform;
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                devilTarget.target = null;
            }
        }        
    }
}
