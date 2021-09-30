using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class KETweapons : MonoBehaviour
{
    
    [Header("Gun Properties")]
    [SerializeField][Tooltip("How far the player can aim the gun")] 
    private float m_maxDistance = 100f;
    [SerializeField] [Tooltip("How much impact is applied to the enemy when hit")] 
    private float impactForce = 100f;
    [SerializeField] [Tooltip("Bullet Speed")] 
    private float bulletVelocity = 5000f;
    [SerializeField] private int gunCost = 10;
    [SerializeField] [Tooltip("Rocket Speed")] 
    private float rocketVelocity = 100;

    [Header("Energy")]
    public int maxEnergy = 100;
    public float currentEnergy;

    [Header("Shield")]
    [SerializeField] private int shieldCost;
    [SerializeField] private float shieldRunTime = 20f;

    [Header("KET Blast Properties")]
    [SerializeField] [Tooltip("How big the blast radius is")] 
    private float shieldRadius = 15f;
    [SerializeField] private float blastUpwardLift = 15f;

    [Header("Audio")]
    [SerializeField] private AudioClip blasterAudioClip;
    [SerializeField] private AudioClip forceFieldAudioClip;
    [SerializeField] private AudioMixerGroup mixer;
    private AudioSource blasterSource;
    private AudioSource shieldSource;
    private GameObject sourceObject;
    [SerializeField] private float blasterMasterVol;

    [Header("Required Objects")]
    [SerializeField] private GameObject forceField;
    [SerializeField] private GameObject ketGunTip;
    [SerializeField] private Transform rocketLauncher;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private LayerMask enemy;
    [SerializeField] [Tooltip("Shield Animator")] 
    private Animator m_shieldAnim;
    private GameObject rocketInstance;

    [Header("Script References")]
    [SerializeField] private DustDevilDamage DustDevilDamage;
    private WeaponSwitcher WeaponSwitcher;
    private EnemyHealth enemyHealth;
    private PlayerHealth playerHealth;

    [HideInInspector] public bool shieldRunning = false;
    private bool blastRunning = false;    

    // Start is called before the first frame update
    void Start()
    {
        forceField.SetActive(false);
        currentEnergy = maxEnergy;
        WeaponSwitcher = GetComponent<WeaponSwitcher>();
        playerHealth = GetComponent<PlayerHealth>();
        sourceObject = transform.Find("Audio").gameObject;
        blasterSource = SetUpAudioSource(blasterAudioClip);
        shieldSource = SetUpAudioSource(forceFieldAudioClip);
        blasterSource.volume = blasterMasterVol;
    }

    // Update is called once per frame
    void Update()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        //KET Blast
        if (Input.GetKeyDown(KeyCode.Q) && !shieldRunning && WeaponSwitcher.weaponTwo && currentEnergy >= shieldCost)
        {
            StartCoroutine(PlayBlastShield(2));
        }

        //KET Gun
        if (Input.GetKeyDown(KeyCode.E) && WeaponSwitcher.weaponTwo && currentEnergy >= gunCost)
        {
            KETgun();
        }

        //Primary Shield
        if (Input.GetKeyDown(KeyCode.E) && !shieldRunning && WeaponSwitcher.weaponThree && !blastRunning && currentEnergy >= shieldCost)
        {
            StartCoroutine(ShieldPrimary());
            shieldRunning = true;
        }

        //Rocket
        if (Input.GetKeyDown(KeyCode.Q) && WeaponSwitcher.weaponOne)
        {
            Rocket(shieldCost);
        }
    }

    private AudioSource SetUpAudioSource(AudioClip clip)
    {
        AudioSource source = sourceObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.outputAudioMixerGroup = mixer;
        source.loop = false;
        source.playOnAwake = false;
        return source;
    }

    public void KETblast(Vector3 center, float radius, float upwardLift)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, enemy);
        Debug.Log(hitColliders.Length);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Enemy") || hitColliders[i].CompareTag("EnemyTruck"))
            {
                EnemyHealth health = hitColliders[i].GetComponent<EnemyHealth>();
                health.TakeDamage(50);
            }
            
            hitColliders[i].attachedRigidbody.AddExplosionForce(impactForce, center, radius, upwardLift);
        }
    }

    public void KETgun()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GameObject bulletInstance;

        if (currentEnergy > gunCost)
        {
            //Spawn a bullet, deduct the cost and play the associated clip.
            TrySpendEnergy(gunCost);
            bulletInstance = Instantiate(bullet, ketGunTip.transform.position, ketGunTip.transform.rotation);
            bulletInstance.transform.rotation = Quaternion.Euler(90, 0, 0);
            blasterSource.Play();

            if (Physics.Raycast(ray, out RaycastHit hit, m_maxDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player Clicked");
                    return;
                }

                //Get the direction to the target and add force to the bullet.
                Vector3 bulletDir;
                bulletDir = (hit.point - ketGunTip.transform.position).normalized;
                bulletInstance.GetComponent<Rigidbody>().AddForce(100 * bulletVelocity * Time.deltaTime * bulletDir);

                //If the raycast hit an enemy add force to their rigidbody and do some damage.
                if (hit.rigidbody != null && hit.collider.CompareTag("Enemy"))
                {
                    enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                    enemyHealth.TakeDamage(20);
                }
            }
            else
            {
                //If the raycast doesn't hit anything move the bullet forward.
                bulletInstance.GetComponent<Rigidbody>().AddForce(100 * bulletVelocity * Time.deltaTime * transform.forward);
            }
        }
    }

    //Play shield animation
    public void PlayShield()
    {
        if (forceField != null)
        {
            if (m_shieldAnim != null)
            {
                forceField.SetActive(true);
                bool isPlaying = m_shieldAnim.GetBool("isActive");
                m_shieldAnim.SetBool("isActive", !isPlaying);
            }
        }
    }

    //Draw a sphere to visualize the KET blast radius
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shieldRadius);
        }
    }

    /// <summary>
    /// Regenerates energy over time by regenAmount.
    /// </summary>
    /// <param name="regenAmount"></param>
    public void EnergyRegen(float regenAmount)
    {
        currentEnergy += regenAmount * Time.deltaTime;
        playerHealth.HealthBar();
    }

    /// <summary>
    /// Deducts amount from current energy.
    /// </summary>
    /// <param name="amount"></param>
    public void TrySpendEnergy(int amount)
    {
        currentEnergy -= amount;
        playerHealth.HealthBar();
    }

    /// <summary>
    /// Returns current energy normalized (Between 0 and 1).
    /// </summary>
    /// <returns></returns>
    public float GetEnergyNormalized()
    {
        //return the current energy as normalized so it can be passed into a UI slider component
        return currentEnergy / maxEnergy;
    }

    //Coroutine to start the shield animation for the KET blast
    IEnumerator PlayBlastShield(float waitTime)
    {
        blastRunning = true;
        shieldSource.Play();
        TrySpendEnergy(shieldCost);
        PlayShield();
        KETblast(transform.position, shieldRadius, blastUpwardLift);
        yield return new WaitForSeconds(waitTime);
        PlayShield();
        blastRunning = false;
        shieldSource.Stop();
    }

    /// <summary>
    /// Spend energy overtime
    /// </summary>
    /// <param name="cost"></param>
    public void SpendEnergy(float cost)
    {
        currentEnergy -= cost * Time.deltaTime;
        playerHealth.HealthBar();
    }

    //Method for firing the rocket
    void Rocket(float rocketCost)
    {
        if (currentEnergy > rocketCost)
        {
            blasterSource.Play();
            rocketInstance = Instantiate(rocketPrefab, rocketLauncher.transform.position, Quaternion.identity);
            rocketInstance.GetComponent<Rigidbody>().AddForce(transform.forward * rocketVelocity);
            TrySpendEnergy(shieldCost);
        }        
    }

    //Method for adding energy to the pool
    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        playerHealth.HealthBar();
    }

    //Coroutine to enable the shield
    IEnumerator ShieldPrimary()
    {
        shieldSource.loop = true;
        shieldSource.Play();
        TrySpendEnergy(shieldCost);
        PlayShield();
        yield return new WaitForSeconds(shieldRunTime);
        shieldSource.loop = false;
        shieldSource.Stop();
        shieldRunning = false;
    }
}
