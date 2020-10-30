// Copyright (c) 2020 by Yuya Yoshino

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player = null;

    public float smoothSpeed = 0.25f;
    public float cameraHeight;

    private Vector3 targetOffset;
    private Vector3 currentOffset;

    private void Start()
    {
        targetOffset = new Vector3(0, cameraHeight, 0);
        currentOffset = targetOffset;
    }

    private void FixedUpdate()
    {
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, smoothSpeed);
    }

    private void Update()
    {
        transform.position = _player.position + currentOffset;
    }
}
