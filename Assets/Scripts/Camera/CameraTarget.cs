using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraTarget : MonoBehaviour
{
    public Vector3 bias = Vector3.up;
    public float multiplier = 1f;
    public float stiffness = 1f;

    private Transform player;
    private Transform cameraTransform;
    private CharacterController playerController;

    private Vector3 currentVelocityOffset = new Vector3();

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        playerController = player.GetComponent<CharacterController>();

        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 targetOffset = playerController.velocity*multiplier;
        currentVelocityOffset = Vector3.Lerp(currentVelocityOffset, targetOffset,
                                             Time.deltaTime*stiffness);

        Vector3 euler = cameraTransform.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        Quaternion rotationMatrix = Quaternion.Euler(euler);

        transform.position = player.position + rotationMatrix*bias +
                             currentVelocityOffset;
    }
}
