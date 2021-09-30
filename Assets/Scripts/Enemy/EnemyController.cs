using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private AiCar aiCar;
    private Rigidbody rb;
    private EnemyTarget enemyTarget;
    private NavMeshPath path;
    private Transform navMeshAgent;

    [SerializeField] private float steeringSensitivity = 0.01f;
    [SerializeField] private float accelerationSensitivity = 0.3f;
    [SerializeField] private float lookAhead = 10f;
    private float lastTimeMoving = 0;
    private int unstuckCount = 0;

    private Vector3 currentWaypoint;
    private Vector3 lastKnownPosition;

    private void Start()
    {
        path = new NavMeshPath();
        aiCar = GetComponent<AiCar>();
        rb = GetComponent<Rigidbody>();
        enemyTarget = GameObject.Find("Game Manager").GetComponent<EnemyTarget>();
        currentWaypoint = transform.position;
        navMeshAgent = transform.Find("Agent");
    }

    private void Update()
    {
        ProgressTracker();
        Drive();
        Reverse();
    }

    bool FindNavMesh(out Vector3 result)
    {
        //Find a spot on the NavMesh around the enemy.
        if (NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * 10f, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    void ProgressTracker()
    {
        if (enemyTarget.target != null)
        {
            //When the enemy is close to its current waypoint calculate a new path.
            if (Vector3.Distance(transform.position, currentWaypoint) <= lookAhead)
            {
                //Try to calculate a path to the target. If that fails try to get a path to the last known position.
                if (NavMesh.CalculatePath(navMeshAgent.position, enemyTarget.target.position, NavMesh.AllAreas, path))
                {
                    currentWaypoint = path.corners[1];
                    lastKnownPosition = enemyTarget.target.position;
                }
                else if (NavMesh.CalculatePath(navMeshAgent.position, lastKnownPosition, NavMesh.AllAreas, path))
                {
                    currentWaypoint = path.corners[1];
                }
                else
                {
                    //If finding any path fails find a new spot on the NavMesh.
                    if (FindNavMesh(out Vector3 hit))
                    {
                        transform.position = hit;
                    }
                    else
                    {
                        Debug.LogError("Totally Lost -> " + gameObject.name);
                    }
                }
            }
        }
    }

    void Drive()
    {
        if (enemyTarget.target != null)
        {
            // Create and build our values.
            Vector3 localTarget;
            float targetAngle;

            localTarget = aiCar.transform.InverseTransformPoint(currentWaypoint);
            targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(aiCar.current_speed);
            float speedFactor = aiCar.current_speed / 20;
            float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
            float cornerFactor = corner / 90f;
            float brake = 0;
            float accel = 1f;

            if (corner > 20 && speedFactor > 0.1f && speedFactor > 0.2f)
                accel = Mathf.Lerp(0, 1 * accelerationSensitivity, 1 - cornerFactor);

            //Pass the values into our car
            aiCar.AccelerateCart(accel, steer, brake);
        }        
    }

    void Reverse()
    {
        //If our car gets stuck start a timer. Once the timer is up try moving the car backwards.
        //If that doesn't get the car unstuck find a new position on the NavMesh.
        if (enemyTarget.target != null)
        {
            if (rb.velocity.magnitude > 1)
            {
                lastTimeMoving = Time.time;
            }

            if (unstuckCount >= 100)
            {
                if (FindNavMesh(out Vector3 hit))
                {
                    transform.position = hit;
                    unstuckCount = 0;
                    Debug.Log("found NavMesh - " + gameObject.name);
                }
            }

            if (Time.time > lastTimeMoving + 30)
            {
                rb.MovePosition(transform.position - transform.forward * 1.5f);
                unstuckCount++;
            }
        }        
    }
}
