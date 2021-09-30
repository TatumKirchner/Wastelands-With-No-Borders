using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLights : MonoBehaviour
{
    [SerializeField] private Light lightOne, lightTwo;
    private bool lightsOn = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            lightsOn = !lightsOn;
            LightSwitch();
        }        
    }

    // Toggle the lights
    void LightSwitch()
    {
        if (!lightsOn)
        {
            lightOne.enabled = false;
            lightTwo.enabled = false;
        }
        else
        {
            lightOne.enabled = true;
            lightTwo.enabled = true;
        }
    }
}
