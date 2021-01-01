using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraControllerA : MonoBehaviour
{
    public float stiffness = 0.2f;

    public float distance;
    public float pitch;
    public float radialAngle;

    public float sensorAngle = 30f;
    public float mouseMultiplier = 1f;
    public float recoveryMultiplier = 1.5f;
    public float angularDeceleration = 80f;
    public float angularAcceleration = 80f;
    public float angularMaxSpeed = 80f;

    public Transform subject;

    private Vector3 currentTarget;
    private Transform target;

    private float angularVelocity = 0f;

    private bool sensor1Active = false;
    private bool sensor2Active = false;
    private int cameraObstaclesLayerMask;

    private void Start()
    {
        target = GameObject.Find("Camera Target").transform;
        currentTarget = target.position;

        cameraObstaclesLayerMask = LayerMask.GetMask("Camera Obstacles");
    }

    private void Move()
    {
        currentTarget = Vector3.Lerp(currentTarget, target.position,
                                     stiffness*Time.deltaTime);

        Quaternion pitchMatrix = Quaternion.Euler(pitch, 0, 0);
        Quaternion radialMatrix = Quaternion.Euler(0, radialAngle, 0);

        float pitchRad = Mathf.Deg2Rad*pitch;

        Vector3 offset = new Vector3();
        offset.y = Mathf.Sin(pitchRad)*distance;
        offset.z = -Mathf.Cos(pitchRad)*distance;
        offset.x = 0;

        offset = radialMatrix*offset;

        transform.position = currentTarget + offset;
        transform.rotation = radialMatrix*pitchMatrix;
    }

    private bool SensorRay(Ray ray, float currentDistance)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, cameraObstaclesLayerMask))
        {
            if (hitInfo.distance < currentDistance)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    private void Survey()
    {
        Vector3 origin = subject.position;
        Vector3 baseDirection = transform.position - currentTarget;

        Quaternion angleMatrix = Quaternion.Euler(0, sensorAngle, 0);
        Vector3 dir1 = angleMatrix*baseDirection;
        Vector3 dir2 = Quaternion.Inverse(angleMatrix)*baseDirection;

        Ray ray1 = new Ray(origin, dir1);
        Ray ray2 = new Ray(origin, dir2);

        float currentDistance = Vector3.Distance(transform.position, currentTarget);
        sensor1Active = SensorRay(ray1, currentDistance);
        sensor2Active = SensorRay(ray2, currentDistance);
    }

    private void AddForces()
    {
        bool forcesActive = false;
        int sensorDistribution = 0;

        float angularSpeed = Mathf.Abs(angularVelocity);

        if (angularSpeed <= angularMaxSpeed)
        {
            if (sensor1Active)
            {
                angularVelocity -= angularAcceleration*Time.deltaTime;
                ++sensorDistribution;
            }

            if (sensor2Active)
            {
                angularVelocity += angularAcceleration*Time.deltaTime;
                --sensorDistribution;
            }
        }

        if (sensorDistribution != 0)
            forcesActive = true;

        if (UserInput.middleMouse && UserInput.mouseVelocity.magnitude > float.Epsilon)
        {
            angularVelocity = UserInput.mouseVelocity.x*mouseMultiplier;
            forcesActive = true;
        }

        if (!forcesActive && angularSpeed > 0f)
        {
            float multiplier = 1f;

            float excess = angularSpeed - angularMaxSpeed;
            if (excess > 0f)
                multiplier = (excess + 1)*recoveryMultiplier;

            float sign = Mathf.Sign(angularVelocity);
            angularVelocity -= sign*angularDeceleration*Time.deltaTime*multiplier;

            float signPrime = Mathf.Sign(angularVelocity);
            if (sign != signPrime)
                angularVelocity = 0f;
        }
    }

    private void AddAngularVelocity()
    {
        radialAngle += angularVelocity*Time.deltaTime;
    }

    private void LateUpdate()
    {
        Move();
        Survey();
        AddForces();
        AddAngularVelocity();
    }
}