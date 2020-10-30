using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProjection : MonoBehaviour
{
    private void Start()
    {
        Transform sun = GameObject.Find("Sun").transform;
        sun.rotation = Quaternion.Euler(135, 0, 0);
    }
}
