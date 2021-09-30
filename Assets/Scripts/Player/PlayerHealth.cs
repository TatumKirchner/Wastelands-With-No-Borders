using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Variables
    private KETweapons KETweapons;
    private Rope rope;
    public GameObject deathScreen;
    [SerializeField] private IsPaused pauseManager;
    [SerializeField] private CameraShake shake;
    [SerializeField] private GameObject[] healthBars;
    [SerializeField] private GameObject redHealthBar;
    [SerializeField][Tooltip("Game manager object that holds the restart script")]
    private Restart Restart;

    public int maxHealth = 100;
    public int currentHealth;
    private int healthLevel;
    [SerializeField] int bulletDamage = 20;
    [SerializeField] int truckDamage = 100;

    [SerializeField] private ParticleSystem explosion;
    [SerializeField] [Tooltip("Meshes for the passive shield")] 
    private GameObject passiveShield, cannonPassiveShield;

    [HideInInspector] public bool dead = false;
    private bool passiveShieldUp;
    public bool deadPs = false;
    private bool alreadyFlashing = false;

    [SerializeField] private float flashWaitTime = 1f;
    [SerializeField] private float shakeDuration = 1f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        KETweapons = GetComponent<KETweapons>();
        currentHealth = maxHealth;
        rope = GetComponent<Rope>();
        redHealthBar.SetActive(false);
    }

    //If the player collides with an enemy bullet or an enemy truck try to do damage.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("EnemyBullet"))
        {
            TakeDamage(bulletDamage);
        }

        if (collision.collider.CompareTag("EnemyTruck"))
        {
            if (KETweapons.currentEnergy <= 0 && !KETweapons.shieldRunning)
            {
                currentHealth -= truckDamage;
            }
        }
    }

    /// <summary>
    /// Updates the health UI.
    /// </summary>
    public void HealthBar()
    {
        SetHealthLevel();

        switch (healthLevel)
        {
            case 1:
                healthBars[0].SetActive(false);
                healthBars[1].SetActive(false);
                healthBars[2].SetActive(false);
                healthBars[3].SetActive(false);
                healthBars[4].SetActive(true);
                break;
            case 2:
                healthBars[0].SetActive(false);
                healthBars[1].SetActive(false);
                healthBars[2].SetActive(false);
                healthBars[3].SetActive(true);
                healthBars[4].SetActive(true);
                break;
            case 3:
                healthBars[0].SetActive(false);
                healthBars[1].SetActive(false);
                healthBars[2].SetActive(true);
                healthBars[3].SetActive(true);
                healthBars[4].SetActive(true);
                break;
            case 4:
                healthBars[0].SetActive(false);
                healthBars[1].SetActive(true);
                healthBars[2].SetActive(true);
                healthBars[3].SetActive(true);
                healthBars[4].SetActive(true);
                break;
            case 5:
                healthBars[0].SetActive(true);
                healthBars[1].SetActive(true);
                healthBars[2].SetActive(true);
                healthBars[3].SetActive(true);
                healthBars[4].SetActive(true);
                break;
            default:
                healthBars[0].SetActive(false);
                healthBars[1].SetActive(false);
                healthBars[2].SetActive(false);
                healthBars[3].SetActive(false);
                healthBars[4].SetActive(false);
                redHealthBar.SetActive(true);
                break;
        }
    }

    //Checks what the player health is and sets the health level to update the UI.
    void SetHealthLevel()
    {
        if (KETweapons.currentEnergy >= 100)
            healthLevel = 5;
        if (KETweapons.currentEnergy <= 80 && KETweapons.currentEnergy >= 61)
            healthLevel = 4;
        if (KETweapons.currentEnergy <= 60 && KETweapons.currentEnergy >= 41)
            healthLevel = 3;
        if (KETweapons.currentEnergy <= 40 && KETweapons.currentEnergy >= 21)
            healthLevel = 2;
        if (KETweapons.currentEnergy <= 21 && KETweapons.currentEnergy >= 1)
            healthLevel = 1;
        if (KETweapons.currentEnergy <= 0)
            healthLevel = 0;
    }

    //If the player is out of energy turn off the shield on the player.
    void TogglePassiveShield()
    {
        if (KETweapons.currentEnergy > 0)
        {
            passiveShield.SetActive(true);
            cannonPassiveShield.SetActive(true);
            passiveShieldUp = true;
        }
        else
        {
            passiveShield.SetActive(false);
            cannonPassiveShield.SetActive(false);
            passiveShieldUp = false;
        }
    }

    /// <summary>
    /// Call when the player takes damage. Will shake the camera and make the health bar flash.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        shake.shakeDuration = shakeDuration;

        if (!alreadyFlashing)
        {
            StartCoroutine(HealthBarFlash());
        }        

        if (!KETweapons.shieldRunning && passiveShieldUp)
        {
            KETweapons.TrySpendEnergy(damage);
        }
        if (!passiveShieldUp && !KETweapons.shieldRunning)
        {
            currentHealth -= damage + 30;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
            {
                if (rope.hooked || rope.swingHooked || rope.enemyHooked)
                {
                    rope.DestroyRope();
                }
                dead = true;
                if (!deadPs)
                {
                    ParticleSystem clone = Instantiate(explosion, transform.position, Quaternion.identity);
                    Destroy(clone, 1);
                    deadPs = true;
                }

                deathScreen.SetActive(true);
                pauseManager.paused = true;
            }
        }

        HealthBar();
        TogglePassiveShield();
    }

    /// <summary>
    /// Call to add health to the player.
    /// </summary>
    /// <param name="amount"></param>
    public void AddHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthBar();
    }

    //Will make the health bar UI flash when the player takes damage
    IEnumerator HealthBarFlash()
    {
        if (!alreadyFlashing)
        {
            alreadyFlashing = true;
            if (KETweapons.currentEnergy <= 0)
            {
                redHealthBar.SetActive(false);
            }
            else
            {
                redHealthBar.SetActive(true);
            }

            yield return new WaitForSeconds(flashWaitTime);

            if (KETweapons.currentEnergy > 0)
            {
                redHealthBar.SetActive(false);
            }
            else
            {
                redHealthBar.SetActive(true);
            }

            alreadyFlashing = false;
            HealthBar();
        }    
    }
}
