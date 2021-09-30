using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip transitionClip;
    [SerializeField] private AudioClip combatClip;
    [SerializeField] private AudioClip drivingClip;
    [SerializeField] private float fadeSpeed = .1f;

    [HideInInspector] public bool combatStart = false;
    [SerializeField] private bool startingCombat;
    [SerializeField] private bool endingCombat;
    public bool transitionPlayed = false;
    public bool combatEnd = false;
    public bool fullVolume = true;

    private void OnTriggerEnter(Collider other)
    {
        //Depending on the bool that is set in editor either start combat music or end combat music.
        if (other.CompareTag("Player"))
        {
            if (startingCombat)
            {
                combatStart = true;
                combatEnd = false;
            }
            if (endingCombat)
            {
                combatEnd = true;
                combatStart = false;
            }            
        }               
    }

    private void Update()
    {
        SwitchClips();
    }

    void SwitchClips()
    {
        //At the start of combat turn looping off.
        if (combatStart)
        {
            source.loop = false;
        }
        //When combat ends turn loop off and lerp the volume down to 0
        if (combatEnd)
        {
            source.loop = false;
            source.volume = Mathf.Lerp(source.volume, 0, Time.deltaTime * fadeSpeed);
        }

        //Check if the transition clip played.
        if (source.clip == transitionClip && source.time >= transitionClip.length)
        {
            transitionPlayed = true;
        }

        if (!source.isPlaying && combatStart)
        {
            //If the transition hasn't played, play it. Otherwise start the combat music.
            if (!transitionPlayed)
            {
                source.clip = transitionClip;
                source.Play();              
            }
            else
            {
                source.clip = combatClip;
                source.loop = true;
                source.Play();
                combatStart = false;
            }            
        }

        //If we are not at full volume, lerp the volume back up.
        if (!fullVolume)
        {
            source.volume = Mathf.Lerp(source.volume, 1, Time.deltaTime * fadeSpeed);
        }

        //If the volume is close enough to full stop lerping it up.
        if (source.volume >= .9 && !fullVolume)
        {
            fullVolume = true;
            source.volume = 1;
        }

        //If the volume is close enough to 0 swap the clip back to the normal one, start lerping volume back up, and set the source to loop.
        if (source.volume <= .1)
        {
            combatEnd = false;
            fullVolume = false;
            source.loop = true;
            source.clip = drivingClip;            
            source.Play();
        }
    }
}
