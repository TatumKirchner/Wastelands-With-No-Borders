using System;
using System.Net.Sockets;
using Unity.Mathematics;
using UnityEngine;

public class CarSelfRighting : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] private float m_WaitTime = 3f; // time to wait before self righting

    private float m_LastOkTime; // the last time that the car was in an OK state
    public bool carNeedsHelp = false;
    private CarController carController;
    private Quaternion normal;
    [SerializeField] private bool isAiCar;


    private void Start()
    {
        if (!isAiCar)
        {
            carController = GetComponent<CarController>();
        }
    }


    private void Update()
    {
        // If the car is not the right way up start LastOkTime timer
        if (transform.up.y > 0f)
        {
            m_LastOkTime = Time.time;
            
        }
        else
        {
            carNeedsHelp = true;
        }

        //When the timer limit is hit right the car
        if (Time.time > m_LastOkTime + m_WaitTime)
        {
            RightCar();
        }
    }

    private void FixedUpdate()
    {
        KeepCarLevel();
    }


    // put the car back the right way up:
    private void RightCar()
    {
        // set the correct orientation for the car, and lift it off the ground a little
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }

    // When the car is in the air keep it's rotation close to what it was when leaving the ground
    void KeepCarLevel()
    {
        if (!isAiCar)
        {
            for (int i = 0; i < 4; i++)
            {
                if (carController.m_WheelColliders[i].isGrounded)
                {
                    normal = transform.rotation;
                    carNeedsHelp = false;
                }
                if (!carController.m_WheelColliders[i].isGrounded && !carNeedsHelp)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, normal, 1f * Time.deltaTime);
                }
            }
        } 
    }
}
