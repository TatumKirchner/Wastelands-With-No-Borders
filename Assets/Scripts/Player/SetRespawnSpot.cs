using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRespawnSpot : MonoBehaviour
{
    private Restart restart;

    private void Start()
    {
        restart = GameObject.Find("Game Manager").GetComponent<Restart>();
    }

    //When the player enters the trigger set it as the respawn point
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            restart.currentRespawnPoint = transform;
        }
    }
}
