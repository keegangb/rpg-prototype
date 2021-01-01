/* 
 * Copyright (c) 2020, Yuya Yoshino
 * Copyright (c) 2020, Keegan Beaulieu
 */

using UnityEngine;

public class PlayerMovement : Action
{
    public float movespeed = 6f;
    public float acceleration = 1f;
    public float friction = 1f;
    public float correction = 1f;
    public float orientationAlpha = 0.1f;

    private PhysicsBody physicsBody;
    private Transform cameraTransform;
    private SmoothRotation smoothRotation;

    private bool canMove = false;

    public override void OnActionBegin()
    {
        canMove = true;
    }

    public override void OnActionCancel()
    {
        canMove = false;
    }

    public override void OnActionUpdate()
    {
        if (UserInput.movement == Vector2.zero)
            CancelAction();
    }

    protected override void Start()
    {
        base.Start();

        actionExclusive = false;
        actionString = "Movement";

        physicsBody = GetComponent<PhysicsBody>();
        smoothRotation = GetComponent<SmoothRotation>();
        cameraTransform = Camera.main.transform;
    }

    private void Accelerate()
    {
        // Target velocity
        Vector3 targetVelocity;

        if (canMove)
        {
            // Get base target
            targetVelocity = new Vector3(UserInput.movement.x, 0,
                                         UserInput.movement.y);
            targetVelocity.Normalize();
            targetVelocity *= movespeed;

            // Rotate target to align with camera
            float cameraY = cameraTransform.eulerAngles.y;
            Quaternion cameraYawMatrix = Quaternion.Euler(0, cameraY, 0);
            targetVelocity = cameraYawMatrix*targetVelocity;

            // Align player to movement
            if (targetVelocity.magnitude > float.Epsilon)
                smoothRotation.targetDirection = targetVelocity;
        }
        else
        {
            targetVelocity = Vector3.zero;
        }

        Vector3 horizontalVelocity = physicsBody.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;
        Vector3 difference = targetVelocity - horizontalVelocity;
        Vector3 differenceSigns = new Vector3(Mathf.Sign(difference.x), 0,
                                              Mathf.Sign(difference.z));

        // How much acceleration can we preform this tick
        float xDelta, zDelta;

        if (targetVelocity.x != 0)
            xDelta = acceleration;
        else
            xDelta = friction*Mathf.Abs(horizontalVelocity.x);

        if (targetVelocity.z != 0)
            zDelta = acceleration;
        else
            zDelta = friction*Mathf.Abs(horizontalVelocity.z);

        if (speed > movespeed)
        {
            xDelta *= (speed - movespeed)*correction + 1f;
            zDelta *= (speed - movespeed)*correction + 1f;
        }

        xDelta *= Time.deltaTime;
        zDelta *= Time.deltaTime;

        // If we can reach target, set it, otherwise approach it
        if (xDelta >= Mathf.Abs(difference.x))
            physicsBody.velocity.x = targetVelocity.x;
        else
            physicsBody.velocity.x += xDelta*differenceSigns.x;

        if (zDelta >= Mathf.Abs(difference.z))
            physicsBody.velocity.z = targetVelocity.z;
        else
            physicsBody.velocity.z += zDelta*differenceSigns.z;
    }

    private void Update()
    {
        if (!canMove && UserInput.movement != Vector2.zero)
            RequestAction();

        Accelerate();
    }
}