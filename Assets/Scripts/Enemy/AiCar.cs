using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCar : MonoBehaviour
{
    public WheelCollider[] wheels;
    public Transform[] wheels_mesh;

    public float wheel_torque = 200;
    public float brake_torque = 500;
    public float max_steerangle = 30;
    public float current_speed;
    public float maxSpeed = 30;

    private Vector3 wheel_position;
    private Quaternion wheel_rotation;

    [HideInInspector]
    public Rigidbody _rigidbody;

    public bool isAICart;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void AccelerateCart(float v, float h, float b)
    {
        //Convert the rigidbody velocity to MPH
        current_speed = Mathf.RoundToInt(_rigidbody.velocity.magnitude * 2.23693629f);

        //Set the motor torque
        if (v > 0)
        {
            if (current_speed < maxSpeed)
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].motorTorque = Mathf.Clamp(v, -1f, 1f) * wheel_torque;
                }
            }
            else
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].motorTorque = 0;
                }
            }
        }
        else if (v < 0)
        {
            if (current_speed > -5)
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].motorTorque = Mathf.Clamp(v, -1f, 1f) * wheel_torque;
                }
            }
            else
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].motorTorque = 0;
                }
            }
        }

        if (v == 0 && !isAICart)
        {
            b = 0.3f;
        }

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].brakeTorque = Mathf.Clamp(b, 0f, 1f) * brake_torque;
        }

        //Set the angle of the front tires
        wheels[0].steerAngle = Mathf.Clamp(h, -1f, 1f) * max_steerangle;
        wheels[1].steerAngle = Mathf.Clamp(h, -1f, 1f) * max_steerangle;
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].GetWorldPose(out wheel_position, out wheel_rotation);
            wheels_mesh[i].position = wheel_position;
            wheels_mesh[i].rotation = wheel_rotation;
        }
    }
}
