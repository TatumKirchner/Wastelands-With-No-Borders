using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSandstorm : MonoBehaviour
{

    // Rotate the game object on its z axis
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 25) * Time.deltaTime);
    }
}
