/* 
 * Copyright (c) 2020, Yuya Yoshino
 * Copyright (c) 2020, Keegan Beaulieu
 */

using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player = null;

    [SerializeField] private float smoothSpeed = 0.25f;
    [SerializeField] private float cameraHeight;

    [SerializeField] private float mouseOffsetMultiplier;

    private Vector3 targetOffset;
    private Vector3 currentOffset;

    private void Start()
    {
        targetOffset = new Vector3(0, _player.position.y + cameraHeight, 0);
        currentOffset = targetOffset;
    }

    private void FixedUpdate()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        Vector2 diff = mousePos - screenCenter;
        Vector2 horizontal = diff*mouseOffsetMultiplier;

        float vertical = _player.position.y + cameraHeight;

        targetOffset = new Vector3(horizontal.x, vertical, horizontal.y);

        currentOffset = Vector3.Lerp(currentOffset, targetOffset, smoothSpeed);
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = _player.position;
        targetPosition = PixelPerfect3D.SnapPos(targetPosition);
        targetPosition.z += targetPosition.y;

        Vector3 offset = currentOffset;
        offset = PixelPerfect3D.SnapPos(offset);

        transform.position = targetPosition + offset;
    }
}
