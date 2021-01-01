using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    public float alpha = 1f;
    public Vector3 targetDirection = Vector3.forward;

    private void LateUpdate()
    {
        Quaternion target = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, target,
                                             alpha*Time.deltaTime);
    }
}
