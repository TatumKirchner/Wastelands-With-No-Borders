using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lookAhead;
    public Transform currentWaypoint;
    public StormWaypoint stormWaypoint;
    public Transform firstWaypoint;

    private void Update()
    {
        ProgressTracker();
    }

    void ProgressTracker()
    {
        if (currentWaypoint != null)
        {
            //Check if the object is close enough to the waypoint to get the next one.
            if (Vector3.Distance(transform.position, currentWaypoint.position) <= lookAhead)
            {
                if (stormWaypoint.nextWaypoint.GetComponent<StormWaypoint>())
                {
                    currentWaypoint = stormWaypoint.nextWaypoint;
                    stormWaypoint = currentWaypoint.GetComponent<StormWaypoint>();
                }
                else
                {
                    currentWaypoint = transform;
                }
            }
            else
            {
                Move();
            }
        }
    }

    void Move()
    {
        //Move the storm towards the current waypoint
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, currentWaypoint.position, speed * Time.deltaTime), 
            Quaternion.Lerp(transform.rotation, currentWaypoint.rotation, speed * Time.deltaTime));
    }
}
