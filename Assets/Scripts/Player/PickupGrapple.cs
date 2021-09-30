using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGrapple : MonoBehaviour
{
    private LineRenderer m_lr;
    [SerializeField] private LayerMask GrapplePickUp;
    [HideInInspector] public Rigidbody m_rb;

    private Vector3 m_grapplePoint;
    private Vector3 m_currentGrapplePosition;
    private Transform player;
    [SerializeField] private Transform gunTip;
    private Transform pickupObject;

    [SerializeField] private float maxGrappleDistance = 100f;
    [SerializeField] private float movementForce = 10000f;

    private Rope rope;

    [HideInInspector] public bool m_hooked = false;

    private void Awake()
    {
        m_lr = GetComponent<LineRenderer>();
        player = transform;
        rope = GetComponent<Rope>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !m_hooked)
        {
            StartGrapple();
        }
        if (Input.GetMouseButtonUp(0) && m_hooked)
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        if (m_hooked && m_rb != null)
        {
            //Set the grappled objects rigidbody to simulate physics.
            m_rb.isKinematic = false;
            m_rb.useGravity = true;

            //Move the grappled object towards the player
            Vector3 pos = Vector3.MoveTowards(m_rb.transform.position, transform.position, movementForce * Time.deltaTime);
            m_rb.MovePosition(pos);
        }
    }

    void StartGrapple()
    {
        //Cast Ray From Mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, GrapplePickUp) && !m_hooked && !rope.hooked)
        {
            //Get info from the object we hit
            m_grapplePoint = hit.collider.gameObject.transform.position;

            m_rb = hit.rigidbody;
            pickupObject = hit.transform;

            //Set our line renderer position count
            m_lr.positionCount = 2;

            //Start our line from the players gun
            m_currentGrapplePosition = gunTip.position;

            m_hooked = true;
        }
    }
    /// <summary>
    /// Stops the grapple by clearing out values and sets the line renderer position count to 0.
    /// </summary>
    public void StopGrapple()
    {
        m_lr.positionCount = 0;
        m_hooked = false;
        m_rb = null;
        pickupObject = null;
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!pickupObject) return;

        if (pickupObject != null)
        {
            //Lerp the line renderer position to the grappled item.
            m_currentGrapplePosition = Vector3.Lerp(m_currentGrapplePosition, m_rb.transform.position, Time.deltaTime * 8f);

            m_lr.SetPosition(0, gunTip.position);
            m_lr.SetPosition(1, m_currentGrapplePosition);
        }
        else
        {
            m_lr.positionCount = 0;
        }
    }


}
