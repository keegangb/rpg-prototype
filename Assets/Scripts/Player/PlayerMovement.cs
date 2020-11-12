/* 
 * Copyright (c) 2020, Yuya Yoshino
 * Copyright (c) 2020, Keegan Beaulieu
 */

using UnityEngine;

public class PlayerMovement : Action
{
    public float movespeed = 6f;
    public float acceleration = 1f;
    public float deceleration = 1f;

    [System.NonSerialized] public int movementDivisor = 1;

    private PhysicsBody physicsBody;

    private bool canMove = false;

    public override void OnActionBegin()
    {
        canMove = true;
    }

    public override void OnActionCancel()
    {
        canMove = false;
    }

    protected override void Start()
    {
        base.Start();

        actionExclusive = false;
        actionString = "Movement";

        physicsBody = GetComponent<PhysicsBody>();
    }

    private void AccelerateAxis(ref float velocity, float maxSpeed, float target)
    {
        float targetMag = Mathf.Abs(target);
        float speed = Mathf.Abs(velocity);

        // To target velocity
        float delta = target - velocity;
        float deltaSign = Mathf.Sign(delta);
        float deltaMag = Mathf.Abs(delta);

        // How much acceleration can we preform this tick
        float possibleDelta = (speed > maxSpeed || targetMag > float.Epsilon) ?
                              acceleration : deceleration;
        possibleDelta *= Time.deltaTime;
        if (speed > maxSpeed)
            possibleDelta += (speed - maxSpeed)/movespeed*acceleration*Time.deltaTime;

        // If we can reach target, set it, otherwise approach it
        if (possibleDelta >= deltaMag)
            velocity = target;
        else
            velocity += deltaSign*possibleDelta;
    }

    private void Accelerate()
    {
        // Target velocity
        float targetx;
        float targetz;

        if (canMove)
        {
            Vector3 targetVelocity = new Vector3(UserInput.movement.x, 0,
                                                 UserInput.movement.y);
            targetVelocity.Normalize();
            targetVelocity *= movespeed;

            if (targetVelocity.magnitude > float.Epsilon)
                transform.forward = targetVelocity;

            targetx = targetVelocity.x;
            targetz = targetVelocity.z;
        }
        else
        {
            targetx = 0;
            targetz = 0;
        }

        // Max velocity
        Vector3 maxVelocity = physicsBody.velocity;
        maxVelocity.y = 0f;
        maxVelocity.Normalize();
        maxVelocity *= movespeed;

        maxVelocity.x = Mathf.Abs(maxVelocity.x);
        maxVelocity.z = Mathf.Abs(maxVelocity.z);

        // Scale with divisor
        maxVelocity /= movementDivisor;
        targetx /= movementDivisor;
        targetz /= movementDivisor;

        // Accelerate
        AccelerateAxis(ref physicsBody.velocity.x, maxVelocity.x, targetx);
        AccelerateAxis(ref physicsBody.velocity.z, maxVelocity.z, targetz);
    }

    private void Update()
    {
        if (!canMove)
            RequestAction();

        Accelerate();
    }
}
