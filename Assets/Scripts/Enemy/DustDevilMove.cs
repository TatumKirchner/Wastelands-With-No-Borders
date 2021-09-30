using UnityEngine;
using UnityEngine.AI;

public class DustDevilMove : MonoBehaviour
{
    private NavMeshAgent agent;
    [Range(1f, 100f)][SerializeField]
    private float wanderDistance = 50f;
    [HideInInspector] public Transform target;
    private Vector3 destination;
    private DustDevilTarget devilTarget;

    private void Start()
    {
        devilTarget = GameObject.Find("Game Manager").GetComponent<DustDevilTarget>();
        agent = GetComponent<NavMeshAgent>();
        Vector3 newPos = RandomNavSphere(transform.position, wanderDistance, -1);
        agent.SetDestination(newPos);
        destination = newPos;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        //If the player is within the attack area move towards the player. Otherwise find a random spot around the enemy to move to.
        if (devilTarget.target != null)
        {
            agent.SetDestination(devilTarget.target.position);
        }
        else
        {
            if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderDistance, -1);
                agent.SetDestination(newPos);
                destination = newPos;
            }
            //When the agent gets close to the current target find a new one.
            if (agent.remainingDistance <= 4f)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderDistance, -1);
                agent.SetDestination(newPos);
                destination = newPos;
            }
        }
    }

    //Find a random spot on the nav mesh within a sphere.
    public static Vector3 RandomNavSphere(Vector3 orgin, float dist, int layermask)
    {
        Vector3 randomRange = Random.insideUnitSphere * dist;
        randomRange += orgin;
        NavMesh.SamplePosition(randomRange, out NavMeshHit navHit, dist, layermask);
        return navHit.position;
    }

    //Visual helper to see the sphere and the destination in editor.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wanderDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(destination, 1);
    }
}
