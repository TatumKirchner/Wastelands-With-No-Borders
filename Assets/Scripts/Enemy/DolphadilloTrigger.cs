using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphadilloTrigger : MonoBehaviour
{
    public GameObject[] dolphadillos;
    public bool follow = false;

    private void OnTriggerEnter(Collider other)
    {
        //When the player enters the trigger either turn the enemies on or off depending on the flag that was set in editor.
        if (other.CompareTag("Player"))
        {
            if (follow)
            {
                foreach (GameObject dolphadillo in dolphadillos)
                {
                    dolphadillo.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject dolphadillo in dolphadillos)
                {
                    dolphadillo.SetActive(false);
                }
            }
        }
    }
}
