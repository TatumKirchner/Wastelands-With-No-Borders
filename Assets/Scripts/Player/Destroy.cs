using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //If a collision with the player occurs destroy the game object.
        if (!collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }  
    }
}
