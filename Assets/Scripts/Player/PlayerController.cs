/* 
 * Copyright (c) 2020, Yuya Yoshino
 * Copyright (c) 2020, Keegan Beaulieu
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController Player;

    public float movespeed = 6f;
    public float jumpPower = 1f;
    public float acceleration = 1f;

    private Vector3 velocity = new Vector3();

    private float horizontalInput;
    private float verticalInput;

    private GroundCheck groundCheck;

    private void Awake()
    {
        Player = GetComponentInChildren<CharacterController>();
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void AccelerateHorizontal()
    {
        Vector3 accelerationDir = new Vector3(horizontalInput, 0, verticalInput);

        Vector3 horizontalVelocity = velocity;
        horizontalVelocity.y = 0.0f;

        if (accelerationDir.magnitude > float.Epsilon)
        {
            accelerationDir.Normalize();

            horizontalVelocity += accelerationDir*acceleration*Time.deltaTime;
            if (horizontalVelocity.magnitude > movespeed)
            {
                horizontalVelocity.Normalize();
                horizontalVelocity *= movespeed;
            }
        }
        else
        {
            float speed = horizontalVelocity.magnitude;
            float newSpeed;

            float delta = acceleration*Time.deltaTime;
            if (delta < speed)
                newSpeed = speed - delta;
            else
                newSpeed = 0;

            horizontalVelocity.Normalize();
            horizontalVelocity *= newSpeed;
        }

        float yvelocity = velocity.y;
        velocity = horizontalVelocity;
        velocity.y = yvelocity;
    }

    private void AccelerateVertical()
    {
        velocity += Physics.gravity*Time.deltaTime;

        if (groundCheck.grounded)
            velocity.y = 0;

        if (Input.GetKey(KeyCode.Space) && groundCheck.grounded)
        {
            velocity.y = jumpPower;
            groundCheck.grounded = false;
        }
    }

    private void Accelerate()
    {
        AccelerateHorizontal();
        AccelerateVertical();
    }

    private void Move()
    {
        Player.Move(velocity*Time.deltaTime);
    }

    private void Update()
    {
        GetInput();
        Accelerate();
        Move();
    }
}
