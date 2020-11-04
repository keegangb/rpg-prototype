/* 
 * Copyright (c) 2020, Yuya Yoshino
 * Copyright (c) 2020, Keegan Beaulieu
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movespeed = 6f;
    public float acceleration = 1f;

    private PhysicsBody physicsBody;

    private void Awake()
    {
        physicsBody = GetComponent<PhysicsBody>();
    }

    private void AccelerateHorizontal()
    {
        Vector3 accelerationDir = new Vector3(UserInput.movement.x, 0,
                                              UserInput.movement.y);

        Vector3 horizontalVelocity = physicsBody.velocity;
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

        float yvelocity = physicsBody.velocity.y;
        physicsBody.velocity = horizontalVelocity;
        physicsBody.velocity.y = yvelocity;
    }

    private void Accelerate()
    {
        AccelerateHorizontal();
    }

    private void Update()
    {
        Accelerate();
    }
}
