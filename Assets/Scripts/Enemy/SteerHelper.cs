using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerHelper : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    private AiCar aiCar;
    public WheelCollider wheelL;
    public WheelCollider wheelR;
    public float antiRollVal = 5000f;
    private float m_OldRotation;

    [SerializeField] private float m_Downforce = 400f;
    [Range(0, 1)] [SerializeField] private float m_SteerHelper;
    [SerializeField] private Vector3 centerOfMassOffset;


    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        aiCar = GetComponent<AiCar>();
        aiCar.wheels[0].attachedRigidbody.centerOfMass = centerOfMassOffset;
    }

    void FixedUpdate()
    {
        //Create and build values
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = wheelL.GetGroundHit(out WheelHit hit);
        if (groundedL)
        {
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;
        }

        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * antiRollVal;

        //Add antiRollForce to the wheels if they are grounded
        if (groundedL)
            m_rigidbody.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);

        if (groundedR)
            m_rigidbody.AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);

        AddDownForce();
        SteeringHelper();
    }

    //Apply down force to the vehicle.
    private void AddDownForce()
    {
        aiCar.wheels[0].attachedRigidbody.AddForce(aiCar.wheels[0].attachedRigidbody.velocity.magnitude * m_Downforce * -transform.up);
    }

    private void SteeringHelper()
    {
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelhit;
            aiCar.wheels[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return; // wheels aren't on the ground so don't realign the rigidbody velocity
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
        {
            var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            m_rigidbody.velocity = velRotation * m_rigidbody.velocity;
        }
        m_OldRotation = transform.eulerAngles.y;
    }
}
