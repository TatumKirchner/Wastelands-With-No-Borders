using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private LineRenderer _lr;
    public Transform gunTip;
    public Transform cam;
    public Transform player;
    private SpringJoint joint;

    private Vector3 _grapplePoint;
    private Vector3 currentGrapplePosition;
    public LayerMask Grappleable;

    public float maxGrappleDistance = 100f;
    public float grappelMoveSpeed;
    public float downwardForce = 100f;
    public float spring = 4.5f;
    public float damper = 7f;
    public float massScale = 4.5f;

    private bool _hooked = false;

    private Rigidbody rb;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            StartGrapple();
        }
        else if (Input.GetButtonUp("Fire1") && _hooked)
        {
            StopGrapple();
        }
    }

    private void FixedUpdate()
    {
        if (_hooked)
        {
            GrappleMoveTowards();
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        //Under Mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Raycast under mouse
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, Grappleable))
        {
            //Add a spring joint to the player
            _grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = _grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, _grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Set spring joint values
            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;

            //Set line renderer position count.
            _lr.positionCount = 2;

            currentGrapplePosition = gunTip.position;

            _hooked = true;
        }
    }   
    
    //Pull the player towards the grapple point.
    void GrappleMoveTowards()
    {
        if (Vector3.Distance(transform.position, _grapplePoint) > 20f)
        {
            transform.LookAt(_grapplePoint);
            rb.AddRelativeForce(Vector3.forward * grappelMoveSpeed, ForceMode.Acceleration);
        }
        //When the player gets close to the grapple point stop the grapple.
        if (Vector3.Distance(transform.position, _grapplePoint) < 20f)
        {
            StopGrapple();
        }
    }
    
    //Set line renderer position count to 0, destroy the joint, and add a little force to the player to help keep them level on detach.
    void StopGrapple()
    {
        _lr.positionCount = 0;
        Destroy(joint);
        _hooked = false;
        rb.AddForce(-Vector3.up * downwardForce, ForceMode.Impulse);
    }


    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        float dist = Vector3.Distance(gunTip.position, _grapplePoint);

        //If the player is to far away from the grapple point stop the grapple.
        if (dist > 100f)
        {
            StopGrapple();
        }
        else
        {
            //Set the line renderer positions to the player and the current grapple position.
            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, _grapplePoint, Time.deltaTime * 8f);

            _lr.SetPosition(0, gunTip.position);
            _lr.SetPosition(1, currentGrapplePosition);
        }
    }

    /// <summary>
    /// Is the player currently grappling.
    /// </summary>
    /// <returns>
    /// True if there is a joint.
    /// </returns>
    public bool IsGrappling()
    {
        return joint != null;
    }

    /// <summary>
    /// Get the Vector3 of the current grapple point.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetGrapplePoint()
    {
        return _grapplePoint;
    }
}
