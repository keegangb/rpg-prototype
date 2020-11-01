using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

public class SceneProjection : MonoBehaviour
{
    private void Start()
    {
        Transform sun = GameObject.Find("Sun").transform;
        sun.Rotate(Vector3.right, 45, Space.World);
    }
}
