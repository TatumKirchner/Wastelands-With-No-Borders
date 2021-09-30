using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEffects : MonoBehaviour
{
    private WormAI bossAI;
    private Animator mouthAnim;

    [Header("Effects")]
    [SerializeField] ParticleSystem dirtEffect = default;
    [SerializeField] ParticleSystem waterEffect = default;
    ParticleSystem enterParticle, exitParticle;

    void Start()
    {
        mouthAnim = transform.GetChild(0).GetComponent<Animator>();
        bossAI = GetComponent<WormAI>();
        bossAI.GroundContact.AddListener((boolA, boolB) => GroundContact(boolA, boolB));
        bossAI.GroundDetection.AddListener((x, y) => GroundParticleChange(x, y));

        enterParticle = dirtEffect;
        exitParticle = dirtEffect;
    }

    void GroundParticleChange(bool start, int particle)
    {
        if (start)
            enterParticle = particle == 0 ? dirtEffect : waterEffect;
        else
            exitParticle = particle == 0 ? dirtEffect : waterEffect;
    }

    void GroundContact(bool state, bool start)
    {
        if (start)
        {
            if (state)
            {
                //Move the particle systems where the worm comes out of the ground and play it. Also start the animation on the worm.
                enterParticle.transform.position = Vector3.Lerp(bossAI.startPosition, bossAI.endPosition, .1f);
                enterParticle.Play();
                mouthAnim.SetBool("mouthAnim", true);
            }
            else
            {
                enterParticle.Stop();
            }
        }
        else
        {
            //Move the particle system where the worm will enter the ground and play it. Stop the animation.
            if (state)
            {
                exitParticle.transform.position = Vector3.Lerp(bossAI.endPosition, bossAI.startPosition, .22f);
                exitParticle.Play();
                mouthAnim.SetBool("mouthAnim", false);
            }
            else
            {
                exitParticle.Stop();
            }
        }
    }
}
