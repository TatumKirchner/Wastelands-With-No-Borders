using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Rope : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float m_maxRopeDistance = 100f;
    [SerializeField][Tooltip("How many joint are added between the player and the grapple point")][Range(2f, 7f)]
    private int links = 6;
    [SerializeField][Tooltip("Amount of damage to apply to an enemy when grappled")] 
    private float m_damageOverTime;
    [SerializeField][Tooltip("Amount of energy to regain overtime when an enemy is grappled")] 
    private float energyRegen = 5f;

    [Header("Forces")]
    [SerializeField][Tooltip("Amount of down force applied to player when grappling enemies")] 
    private float downForce = 500;
    [SerializeField] private float swingForce = 500;
    [SerializeField] private float upForce;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Swinging Limits")]
    [SerializeField] private float swingCoolDown = 10f;
    [SerializeField] private float swingTime = 2;

    [Header("Audio")]
    [SerializeField] private AudioClip energyGrappleClip;
    private AudioSource grappleAudioSource;
    private GameObject audioObject;
    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] private float grappleMasterVol;

    [Header("Required Components")]
    [SerializeField][Tooltip("The layer the things to grapple are on")] 
    private LayerMask EnemyGrappleable;
    [SerializeField] private Transform m_gunTip;
    [SerializeField] private GameObject linkPrefab;
    [SerializeField] private Transform cannonMesh;

    private GameObject tempLinks;
    private List<GameObject> joints = new List<GameObject>();
    private Rigidbody m_rigidbody;
    private Vector3 m_currentGrapplePosition;
    private Vector3 hookVector;
    private Vector3 hookTransform;
    [SerializeField] private Transform disabledCannonLookAt;
    private Transform hookedTransform;
    private LineRenderer m_lr;
    private Rigidbody hookRB;

    [HideInInspector] public bool hooked = false;
    private bool fired = false;
    [HideInInspector] public bool enemyHooked = false;
    [HideInInspector] public bool swingHooked = false;
    [HideInInspector] public bool swingingForce = false;
    [HideInInspector] public bool canGrapple = true;
    private bool isCannonLooking;

    private int swingCount = 0;
    private float swingCountTimer = 0f;
    private float dist;

    private KETweapons KETweapons;
    private EnemyHealth EnemyHealth;
    private PickupGrapple PickupGrapple;
    private CarController carController;

    private void Awake()
    {
        m_lr = GetComponent<LineRenderer>();
        m_rigidbody = GetComponent<Rigidbody>();
        KETweapons = GetComponent<KETweapons>();
        PickupGrapple = GetComponent<PickupGrapple>();
        carController = GetComponent<CarController>();
        audioObject = transform.Find("Audio").gameObject;
        grappleAudioSource = SetUpAudioSource(energyGrappleClip);
        grappleAudioSource.volume = grappleMasterVol;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hooked)
        {
            StartHook();
        }
        else if (Input.GetMouseButtonUp(0) && hooked)
        {
            DestroyRope();
        }

        EnemyTakeDamage();
        ResetSwingCount();
        DisableCannonPosition();
    }

    private void FixedUpdate()
    {
        ApplyDownForce();
        SwingForce();
    }

    private void LateUpdate()
    {
        if (hooked && !fired)
            GenerateRope();

        TempDrawRope();
    }

    private AudioSource SetUpAudioSource(AudioClip clip)
    {
        AudioSource source = audioObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = mixer;
        return source;

    }

    //Finds the grapple position under cursor
    void StartHook()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (canGrapple)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, m_maxRopeDistance, EnemyGrappleable) && !hooked && !PickupGrapple.m_hooked)
            {
                dist = Vector3.Distance(m_gunTip.position, hit.transform.position);

                grappleAudioSource.Play();

                m_lr.positionCount = 2;
                m_currentGrapplePosition = m_gunTip.position;
                hookRB = hit.collider.GetComponent<Rigidbody>();
                hookTransform = hit.point;
                hookedTransform = hit.transform;
                hookVector = hit.transform.position;

                if (hit.collider.gameObject.layer == 9)
                {
                    hooked = true;
                    if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("EnemyTruck"))
                    {
                        EnemyHealth = hit.collider.GetComponent<EnemyHealth>();
                        enemyHooked = true;
                    }
                    if (hit.collider.CompareTag("Swing"))
                    {
                        swingHooked = true;
                        swingCount += 1;
                    }
                }
            }
        }
    }

    //Instantiates joints between the player and the grapple point and set their values
    void GenerateRope()
    {
        Rigidbody previousRB = hookRB;

        for (int i = 0; i < links; i++)
        {
            tempLinks = Instantiate(linkPrefab, Vector3.Lerp(hookVector, m_currentGrapplePosition, i * .25f), Quaternion.identity);
            joints.Add(tempLinks);
            ConfigurableJoint joint = tempLinks.GetComponent<ConfigurableJoint>();
            joint.connectedBody = previousRB;

            SoftJointLimit jointLimit = new SoftJointLimit
            {
                limit = (dist / links),
                contactDistance = 0.1f,
            };

            joint.linearLimit = jointLimit;

            if (i < links - 1)
            {
                previousRB = tempLinks.GetComponent<Rigidbody>();
            }
            else
            {
                ConnectRopeEnd(tempLinks.GetComponent<Rigidbody>());
            }

            fired = true;
        }
    }

    //When done grappling reset values and destroy the generated joints
    public void DestroyRope()
    {
        grappleAudioSource.Stop();
        m_lr.positionCount = 0;
        hooked = false;
        enemyHooked = false;
        swingHooked = false;
        Destroy(gameObject.GetComponent<ConfigurableJoint>());
        hookTransform = Vector3.zero;
        fired = false;
        swingingForce = false;
        StopCoroutine("Swinging");
        foreach (GameObject joint in joints)
        {
            Destroy(joint);
        }
    }

    //Connect the generated joints to the joint attached to the player
    void ConnectRopeEnd(Rigidbody endRB)
    {
        ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = endRB;
        joint.anchor = new Vector3(0, 0, 0);
        joint.axis = Vector3.up;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = new Vector3(0, 0, 0);
        joint.secondaryAxis = new Vector3(1, 0, 0);
        joint.projectionMode = JointProjectionMode.PositionAndRotation;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit jointLimit = new SoftJointLimit
        {
            contactDistance = 0.1f,
            limit = (dist / links),
        };

        joint.linearLimit = jointLimit;
    }

    //Set line renderer positions depending on what the player has grappled onto
    void TempDrawRope()
    {
        if (!hooked)
            return;

        if (hookTransform != null && swingHooked)
        {
            m_currentGrapplePosition = Vector3.Lerp(m_currentGrapplePosition, hookTransform, Time.deltaTime * 8f);
            m_lr.SetPosition(0, m_gunTip.position);
            m_lr.SetPosition(1, m_currentGrapplePosition);
            cannonMesh.LookAt(hookTransform, Vector3.up);
        }

        if (hookedTransform != null && !swingHooked)
        {
            m_currentGrapplePosition = Vector3.Lerp(m_currentGrapplePosition, hookedTransform.position, Time.deltaTime * 8f);
            m_lr.SetPosition(0, m_gunTip.position);
            m_lr.SetPosition(1, m_currentGrapplePosition);
            cannonMesh.LookAt(hookedTransform, Vector3.up);
        }
    }

    //Add more down force when grappling an enemy to prevent the player from flipping
    void ApplyDownForce()
    {
        if (enemyHooked)
        {
            m_rigidbody.AddForce(-Vector3.up * downForce);

        }
    }

    //Start swinging coroutine to limit the time to add force
    void SwingForce()
    {
        if (swingHooked && !swingingForce && swingCount <= 3)
        {
            StartCoroutine("Swinging");
        }
    }

    //When enemy is grappled they take damage and player regains some energy
    void EnemyTakeDamage()
    {
        if (enemyHooked)
        {
            EnemyHealth.TakeDamageOverTime(m_damageOverTime);
            KETweapons.EnergyRegen(energyRegen);

            if (EnemyHealth.currentHealth <= 0)
            {
                DestroyRope();
            }
        }
    }

    //If player is hooked to a swingable object apply some forces and slowly look towards the hooked point
    IEnumerator Swinging()
    {
        //Rotate towards the hooked point
        Vector3 targetDelta = hookTransform - transform.position;
        float angleDiff = Vector3.Angle(transform.forward, targetDelta);
        Vector3 cross = Vector3.Cross(transform.forward, targetDelta);
        m_rigidbody.AddTorque(angleDiff * rotationSpeed * cross);

        //Add force up and forward for swinging
        m_rigidbody.AddForce(transform.forward * swingForce, ForceMode.Force);
        m_rigidbody.AddForce(Vector3.up * upForce, ForceMode.Force);
        yield return new WaitForSeconds(swingTime);
        swingingForce = true;
    }

    //Limit how many times the player can hook onto a swingable object and have forces applied
    void ResetSwingCount()
    {
        if (swingCount > 1)
        {
            swingCountTimer += Time.deltaTime;

            if (swingCountTimer > swingCoolDown)
            {
                swingCountTimer -= swingCoolDown;
                swingCount = 0;
            }
        }

        if (carController.m_WheelColliders[0].isGrounded)
        {
            swingCount = 0;
        }
    }

    //When the players grappling cannon is disabled the cannon will look down, and when it is restored it will return to the normal position
    void DisableCannonPosition()
    {
        if (!canGrapple && !isCannonLooking)
        {
            cannonMesh.LookAt(disabledCannonLookAt, Vector3.up);
            isCannonLooking = true;
        }
        else
        {
            cannonMesh.rotation = Quaternion.Lerp(cannonMesh.rotation, Quaternion.LookRotation(transform.forward, transform.up), 5f * Time.time);
            isCannonLooking = false;
        }
    }
}
