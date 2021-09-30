using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float rollSpeed = 0.2f;

    [SerializeField] private float spinTurnLimit = 180f;
    [SerializeField] private float velocityLowerLimit = 4f;
    [SerializeField] private float smoothTurnTime = 0.2f;

    private float lastFlatAngle;
    private float currentTurnAmount;
    private float turnSpeedVelocityChange;

    private Vector3 rollUp = Vector3.up;
    private Rigidbody targetRigidbody;
    [SerializeField] private Transform target;

    [SerializeField] private bool followVelocity = false;
    [SerializeField] private bool followTilt = false;

    private void Start()
    {
        targetRigidbody = target.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        Vector3 targetForward = target.forward;
        Vector3 targetUp = target.up;

        if (followVelocity && Application.isPlaying)
        {
            //If the target is moving set targetForward to the targets movement direction.
            if (targetRigidbody.velocity.magnitude > velocityLowerLimit)
            {
                targetForward = targetRigidbody.velocity.normalized;
                targetUp = Vector3.up;
            }
            else
            {
                targetUp = Vector3.up;
            }
            currentTurnAmount = Mathf.SmoothDamp(currentTurnAmount, 1, ref turnSpeedVelocityChange, smoothTurnTime);
        }
        else
        {
            float currentFlatAngle = Mathf.Atan2(targetForward.x, targetForward.z) * Mathf.Rad2Deg;
            if (spinTurnLimit > 0)
            {
                float targetSpinSpeed = Mathf.Abs(Mathf.DeltaAngle(lastFlatAngle, currentFlatAngle)) / Time.deltaTime;
                float desiredTurnAmount = Mathf.InverseLerp(spinTurnLimit, spinTurnLimit * 0.75f, targetSpinSpeed);
                float turnReactSpeed = (currentTurnAmount > desiredTurnAmount ? 0.1f : 1f);

                if (Application.isPlaying)
                {
                    currentTurnAmount = Mathf.SmoothDamp(currentTurnAmount, desiredTurnAmount, ref turnSpeedVelocityChange, turnReactSpeed);
                }
                else
                {
                    currentTurnAmount = desiredTurnAmount;
                }
            }
            else
            {
                currentTurnAmount = 1;
            }
            lastFlatAngle = currentFlatAngle;
        }

        // Move the cameras position
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * moveSpeed);

        if (!followTilt)
        {
            targetForward.y = 0;
            if (targetForward.sqrMagnitude < float.Epsilon)
            {
                targetForward = transform.forward;
            }
        }
        Quaternion rollRotation = Quaternion.LookRotation(targetForward, rollUp);

        // Move the cameras rotation
        rollUp = rollSpeed > 0 ? Vector3.Slerp(rollUp, targetUp, rollSpeed * Time.deltaTime) : Vector3.up;
        transform.rotation = Quaternion.Lerp(transform.rotation, rollRotation, turnSpeed * currentTurnAmount * Time.deltaTime);
    }
}
