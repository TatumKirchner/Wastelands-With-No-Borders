using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroyDelay : MonoBehaviour
{
    //Destroy the particle system after a delay
    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
}
