using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustDevilDamage : MonoBehaviour
{
    private KETweapons KETweapons;
    private PlayerHealth playerHealth;
    private Rope rope;
    private ParticleSystem grappleDisablePs;

    [SerializeField] private int damage = 10;
    [SerializeField] private float grappleDisableTime = 3f;

    private bool playPs = false;

    private void Start()
    {
        rope = FindObjectOfType<Rope>();
        grappleDisablePs = GameObject.Find("Grapple Disable PS").GetComponent<ParticleSystem>();
        KETweapons = GameObject.Find("Player").GetComponent<KETweapons>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnTriggerStay(Collider other)
    {
        //When the player enters the trigger start draining energy
        if (other.CompareTag("Player"))
        {
            if (KETweapons.currentEnergy > 0)
            {
                if (!playPs)
                {
                    grappleDisablePs.Play();
                    playPs = true;
                }
                
                playerHealth.TakeDamage(damage);
            }

            rope.canGrapple = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //When the player leaves the trigger. Disable the grappling hook.
        if (other.CompareTag("Player"))
        {
            playPs = false;
            StartCoroutine(DisableGrapple());
        }
    }

    IEnumerator DisableGrapple()
    {
        rope.canGrapple = false;
        yield return new WaitForSeconds(grappleDisableTime);
        rope.canGrapple = true;
    }
}
