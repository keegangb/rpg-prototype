using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDebugger : MonoBehaviour
{
    private static Camera sceneCamera;
    private static int defaultLayer;

    public static void DrawMesh(Mesh mesh, Material material, Vector3 position,
                                Quaternion rotation, Vector3 scale)
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetTRS(position, rotation, scale);

        Graphics.DrawMesh(mesh, matrix, material, defaultLayer, sceneCamera);
    }

    private void Start()
    {
        sceneCamera = GameObject.Find("Scene Camera").GetComponent<Camera>();
        defaultLayer = LayerMask.NameToLayer("Default");
    }
}