using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShearLight : MonoBehaviour
{
    Vector3 offset;
    float height;

    private void Start()
    {
        offset = transform.localPosition;
    }

    private void Update()
    {
        Vector3 position = transform.parent.position + offset;
        position.z += position.y;
        transform.position = position;
    }
}
