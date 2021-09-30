using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    #region Strings
    private string dispatch1;
    private string dispatch2;
    private string dispatch3;
    private string dispatch4;
    private string dispatch5;
    private string dispatch6;
    private string dispatch7;
    private string dispatch8;
    private string dispatch9;

    private string driver1;
    private string driver2;
    private string driver3;
    private string driver4;
    private string driver5;
    private string driver6;
    private string driver7;
    private string driver8;

    private string billy;
    private string tom;
    private string jack;
    private string willy;
    #endregion

    #region Variables
    [SerializeField] private float timeBetweenText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private CarUserControl userControl;
    [SerializeField] private StormMovement stormMovement;
    #endregion

    #region Bools
    public bool prologue;
    public bool mountainside = false;
    public bool canyon = false;
    public bool shipwreck = false;
    public bool combat = false;

    public bool proloueFinished = false;
    public bool mountainSideFinished = false;
    public bool canyonFinished = false;
    public bool shipWreckFinished = false;
    public bool combatFinished = false;
    #endregion

    private void Start()
    {
        SetDialogue();
        dialogueText.text = null;
        prologue = true;
    }

    //When the flags are triggered start the coroutine.
    private void Update()
    {
        if (prologue)
            StartCoroutine(Prologue());

        if (mountainside && proloueFinished)
            StartCoroutine(Mountainside());

        if (canyon && proloueFinished && mountainSideFinished)
            StartCoroutine(Canyon());

        if (shipwreck && proloueFinished && mountainSideFinished && canyonFinished)
            StartCoroutine(Shipwreck());

        if (combat && proloueFinished && mountainSideFinished && canyonFinished && shipWreckFinished)
            StartCoroutine(Combat());
    }

    /*
     * The coroutines swap out the text then wait a bit before switching to the next text.
     */

    IEnumerator Prologue()
    {
        userControl.enabled = false;
        dialoguePanel.SetActive(true);
        prologue = false;
        dialogueText.text = dispatch1;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = driver1;
        yield return new WaitForSeconds(timeBetweenText);
        userControl.enabled = true;
        dialogueText.text = dispatch2;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = dispatch3;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = dispatch4;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = driver2;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = dispatch5;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = null;
        dialoguePanel.SetActive(false);
        proloueFinished = true;
    }

    IEnumerator Mountainside()
    {
        dialoguePanel.SetActive(true);
        mountainside = false;
        dialogueText.text = driver3;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = dispatch6;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = driver4;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = null;
        dialoguePanel.SetActive(false);
        mountainSideFinished = true;
    }

    IEnumerator Canyon()
    {
        stormMovement.currentWaypoint = stormMovement.firstWaypoint;
        dialoguePanel.SetActive(true);
        canyon = false;
        dialogueText.text = dispatch7;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = driver5;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = null;
        dialoguePanel.SetActive(false);
        canyonFinished = true;
    }

    IEnumerator Shipwreck()
    {
        dialoguePanel.SetActive(true);
        shipwreck = false;
        dialogueText.text = driver6;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = dispatch8;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = driver7;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = dispatch9;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = driver8;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = null;
        dialoguePanel.SetActive(false);
        shipWreckFinished = true;
    }

    IEnumerator Combat()
    {
        dialoguePanel.SetActive(true);
        combat = false;
        dialogueText.text = billy;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = tom;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = jack;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = willy;
        yield return new WaitForSeconds(timeBetweenText);
        dialogueText.text = null;
        dialoguePanel.SetActive(false);
        combatFinished = true;
    }

    //Sets the string variables with their respective text.
    void SetDialogue()
    {
        dispatch1 = "Dispatch - ATTENTION. Dust Storm Warning - take action! Southwest winds 55 to 73 mph with extreme gusts within." +
            " Avoid dust storm area and seek shelter. All without suitable shelter are advised to evacuate immediately to Fort Mount.";
        dispatch2 = "Dispatch - I got you on a private channel and have your location, Driver. Ready?";
        dispatch3 = "Dispatch - A team was out there earlier setting up additional grapple points for just this thing. Using them to maintain speed around the corners and cross large gaps. Should be fairly point and click.";
        dispatch4 = "Dispatch - You know, my great grandfather used to go deep sea fishing around where you're at now, or so my mother says. What do you think it was like back then, before the invasion?";
        dispatch5 = "Dispatch - Ha. Ha.";
        dispatch6 = "Dispatch - Dust Devils?";
        dispatch7 = "Dispatch - I hope you have power to spare for your blaster and shields. That canyon is infested with dolphadillos.";
        dispatch8 = "Dispatch - Yes, it's suspected Rust Raider territory. Is there a way around ? ";
        dispatch9 = "Dispatch - Watch yourself.";

        driver1 = "Driver 23 - Outlands Patrol Driver 23 requesting guidance. Dispatch, storm will likely cross egress - give me an alternate route.";
        driver2 = "Driver 23 - Trickier to drive across.";
        driver3 = "Driver 23 - Hell. I hate these parasites.";
        driver4 = "Driver 23 - Their energy field drains power.";
        driver5 = "Driver 23 - I can't believe anyone used to like these things.";
        driver6 = "Driver 23 - There's a wreck, that on your map?";
        driver7 = "Driver 23 - Only way out is through.";
        driver8 = "Driver 23 - Freaks in rust buckets don't phase me.";

        billy = "Beret Billy - We got a live one - for now! Bitches don't last long on our turf.";
        tom = "Spiketop Tom - Ah, roadkill! My favorite food.";
        jack = "Turban Jack - Which to put through the chop-shop first? Your piece of shit ride or your, shithead.";
        willy = "Windburned Willy - DEMOLITION DERBY! It won't end 'til you stop twitching.";
    }
}
