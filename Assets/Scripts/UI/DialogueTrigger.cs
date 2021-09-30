using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool setMountainside = false;
    public bool setCanyon = false;
    public bool setShipwreck = false;
    public bool setCombat = false;
    private Dialogue dialogue;

    private void Start()
    {
        dialogue = FindObjectOfType<Dialogue>();
    }

    //When the payer enters the trigger start the dialogue that was flagged in the inspector.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (setMountainside)
                dialogue.mountainside = true;
            if (setCanyon)
                dialogue.canyon = true;
            if (setShipwreck)
                dialogue.shipwreck = true;
            if (setCombat)
                dialogue.combat = true;
        }
    }
}
