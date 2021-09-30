using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private EnemyTarget enemyTarget;
    private bool hasBeenSet = false;

    private void OnTriggerEnter(Collider other)
    {
        //When the player enters the trigger set the target for the enemies.
        if (other.gameObject.CompareTag("Player"))
        {
            if (!hasBeenSet)
            {
                enemyTarget.target = other.transform.Find("EnemyTarget");
                hasBeenSet = true;
            }
        }
    }
}
