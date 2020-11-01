// Copyright 2020, Keegan Beaulieu

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SurfaceInstancing : MonoBehaviour
{
    [SerializeField] private GameObject surfacePrefab;
    [SerializeField] private Material surfaceMaterial = null;
    [SerializeField] private float surfaceWidth;

    private Camera sceneCamera = null;

    private ArrayList surfaces = new ArrayList();
    private float xMaxDist;
    private float zMaxDist;

    private void Start()
    {
    #if UNITY_EDITOR
        sceneCamera = GameObject.Find("Scene Camera").GetComponent<Camera>();
        if (!sceneCamera)
            sceneCamera = SceneView.currentDrawingSceneView.camera;
    #else
        sceneCamera = GameObject.Find("Scene Camera").transform;
    #endif

        Vector3 center = sceneCamera.transform.position;
        center.x = Mathf.Round(center.x/surfaceWidth)*surfaceWidth;
        center.y = Mathf.Round(center.y/surfaceWidth)*surfaceWidth;
        center.z = Mathf.Round(center.z/surfaceWidth)*surfaceWidth;

        float sizez, sizex;
        if (sceneCamera.orthographic)
        {
            sizez = sceneCamera.orthographicSize;
            sizex = sceneCamera.orthographicSize*sceneCamera.aspect;
            sizex += surfaceWidth*2f;
            sizez += surfaceWidth*2f;
        }
        else
        {
            sizex = sceneCamera.pixelWidth;
            sizex /= GraphicsConfig.pixelScaleFactor;
            sizex /= GraphicsConfig.unitScaleFactor;

            sizez = (sceneCamera.pixelHeight);
            sizez /= GraphicsConfig.pixelScaleFactor;
            sizez /= GraphicsConfig.unitScaleFactor;
        }
    }

    private void Update()
    {

    }
}
