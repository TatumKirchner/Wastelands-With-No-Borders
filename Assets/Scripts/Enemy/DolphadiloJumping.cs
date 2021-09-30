using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DolphadiloJumping : MonoBehaviour
{
    private Animator anim;
    private CarController player;

    private bool jumping = false;
    private bool normalJump, spinJump;
    private bool jumpFwd, jumpBkwd, jumpSide;

    [SerializeField] private float waitTime = 10f;
    private int jumpType;
    private int jumpAnim;

    private Vector3 startPosition;
    [SerializeField] LayerMask terrainLayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<CarController>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (!jumping)
        {
            StartCoroutine("Jump");
        }
    }

    //Sets the position of the game object
    void SetPositions()
    {
        Vector3 playerPosition = player.transform.position + (player.m_Rigidbody.velocity * 3);
        playerPosition.y = Mathf.Max(10, playerPosition.y);
        Vector3 randomRange = Random.insideUnitSphere * 50;
        randomRange.y = 0;

        //Depending on which animation is going to play move the object to a good starting position
        if (jumpAnim == 1)
        {
            startPosition = player.transform.position + (player.transform.forward + player.m_Rigidbody.velocity * 1.575f);
        }
        if (jumpAnim == 0)
        {
            startPosition = playerPosition + randomRange;
        }

        //If the object starts above the ground, raycast downwards and make that the starting position.
        if (Physics.Raycast(startPosition, Vector3.down, out RaycastHit hit, terrainLayer.value))
        {
            startPosition = hit.point;
        }
    }

    IEnumerator Jump()
    {
        SetPositions();

        //Depending on which animation is going to play tweak the starting position and start the animation. Then pick a new animation to play for the next jump.
        if (jumpFwd)
        {
            jumping = true;
            transform.SetPositionAndRotation(startPosition + (Vector3.down * 5), player.transform.rotation * Quaternion.Euler(0, -90, 0));
            if (normalJump)
                anim.SetBool("Jumping", true);
            if (spinJump)
                anim.SetBool("spinJump", true);
            yield return new WaitForSeconds(waitTime);
            if (normalJump)
                anim.SetBool("Jumping", false);
            if (spinJump)
                anim.SetBool("spinJump", false);
            jumpType = Random.Range(0, 3);
            jumpAnim = Random.Range(0, 2);
            SetJumpType();
            jumping = false;
            yield return new WaitForEndOfFrame();
        }
        if (jumpBkwd)
        {
            jumping = true;
            transform.SetPositionAndRotation(startPosition + (Vector3.down * 5), player.transform.rotation * Quaternion.Euler(0, 90, 0));
            if (normalJump)
                anim.SetBool("Jumping", true);
            if (spinJump)
                anim.SetBool("spinJump", true);
            yield return new WaitForSeconds(waitTime);
            if (normalJump)
                anim.SetBool("Jumping", false);
            if (spinJump)
                anim.SetBool("spinJump", false);
            jumpType = Random.Range(0, 3);
            jumpAnim = Random.Range(0, 2);
            SetJumpType();
            jumping = false;
            yield return new WaitForEndOfFrame();
        }
        if (jumpSide)
        {
            jumping = true;
            transform.SetPositionAndRotation(startPosition + (Vector3.down * 5), player.transform.rotation * Quaternion.Euler(0, 0, 0));
            if (normalJump)
                anim.SetBool("Jumping", true);
            if (spinJump)
                anim.SetBool("spinJump", true);
            yield return new WaitForSeconds(waitTime);
            if (normalJump)
                anim.SetBool("Jumping", false);
            if (spinJump)
                anim.SetBool("spinJump", false);
            jumpType = Random.Range(0, 3);
            jumpAnim = Random.Range(0, 2);
            SetJumpType();
            jumping = false;
        }
        
    }

    //Sets the type of jump and animation to play.
    void SetJumpType()
    {
        if (jumpType == 0)
        {
            jumpFwd = true;
            jumpBkwd = false;
            jumpSide = false;
        }
        if (jumpType == 1)
        {
            jumpFwd = false;
            jumpBkwd = true;
            jumpSide = false;
        }
        if (jumpType == 2)
        {
            jumpFwd = false;
            jumpBkwd = false;
            jumpSide = true;
        }

        if (jumpAnim == 0)
        {
            normalJump = true;
            spinJump = false;
        }
        if (jumpAnim == 1)
        {
            normalJump = false;
            spinJump = true;
        }
    }
}
