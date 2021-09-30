using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;    
    [SerializeField] private ParticleSystem explosionPs;
    private ParticleSystem tempPs;
    public float currentHealth;

    private bool isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    //Called when the enemy takes damage from the player.
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //If the enemy runs out of health play the particle system and turn the enemy off.
        if (currentHealth <= 0)
        {
            tempPs = Instantiate(explosionPs, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            isDestroyed = true;
        }
        if (isDestroyed)
        {
            Destroy(tempPs, 5);
        }
    }

    //Called when the enemy is taking damage over time.
    public void TakeDamageOverTime(float damage)
    {
        currentHealth -= damage * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //Enemy is out of health play a particle system and turn off the enemy.
        if (currentHealth <= 0)
        {
            tempPs = Instantiate(explosionPs, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            isDestroyed = true;
        }
        if (isDestroyed)
        {
            Destroy(tempPs, 5);
        }
    }
}
