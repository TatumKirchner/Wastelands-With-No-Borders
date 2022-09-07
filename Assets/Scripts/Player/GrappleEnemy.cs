using UnityEngine;

public class GrappleEnemy : MonoBehaviour
{
    private LineRenderer m_lr;
    private Vector3 m_currentGrapplePosition;

    public Transform gunTip;

    [SerializeField] private LayerMask grappleSpot;
    [SerializeField] private float maxGrappleDistance = 100f;

    private bool LMB = false;

    public bool m_hooked = false;

    private SpringJoint sJoint;
    private Rope rope;

    private void Awake()
    {
        m_lr = GetComponent<LineRenderer>();
        rope = GetComponent<Rope>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        if (LMB && !m_hooked)
        {
            StartGrapple();
        }
        else
        {
            StopGrapple();
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void GetInput()
    {
        LMB = Input.GetButton("Fire1");
    }

    void StartGrapple()
    {
        //Under Mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Raycast from the mouse position
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, grappleSpot) && !m_hooked && !rope.hooked)
        {
            //Get info from the raycast hit
            if (sJoint == null)
                sJoint = hit.collider.gameObject.GetComponent<SpringJoint>();
            if (sJoint != null && sJoint.connectedBody != null)
                sJoint.connectedBody = GetComponent<Rigidbody>();

            sJoint.connectedAnchor = gunTip.position;

            //Set up the line render position count
            m_lr.positionCount = 2;

            m_currentGrapplePosition = gunTip.position;

            m_hooked = true;
        }
    }

    //Clear out our stored variables and set the line renderer position count.
    void StopGrapple()
    {
        sJoint.connectedBody = null;
        m_lr.positionCount = 0;
        m_hooked = false;
        sJoint = null;
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!sJoint) return;

        //If we are connected draw the line from player to grapple position.
        if (sJoint != null)
        {
            m_currentGrapplePosition = Vector3.Lerp(m_currentGrapplePosition, sJoint.transform.position, Time.deltaTime * 8f);

            m_lr.SetPosition(0, gunTip.position);
            m_lr.SetPosition(1, m_currentGrapplePosition);
        }
        else
        {
            m_lr.positionCount = 0;
        }
    }
}
