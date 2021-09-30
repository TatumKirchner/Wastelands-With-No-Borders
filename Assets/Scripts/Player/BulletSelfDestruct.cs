using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSelfDestruct : MonoBehaviour
{
    [SerializeField] private float destructionTime = 5f;

    //Destroy the bullet after a delay.
    void Start()
    {
        Destroy(gameObject, destructionTime);
    }
}
