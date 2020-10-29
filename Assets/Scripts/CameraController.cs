// Copyright (c) 2020 by Yuya Yoshino

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player = null;

    public float smoothSpeed = 0.25f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        Vector3 desiredPosition = _player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
