// Copyright 2020, Keegan Beaulieu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfect3D : MonoBehaviour
{
    private Material material;
    private int positionId;
    private int cameraPositionId;
    private int unitScaleId;

    private Transform sceneCamera;

    private void Start()
    {
        Shader diffuse = Shader.Find("Custom/PixelDiffuse");
        Shader editorDiffuse = Shader.Find("Custom/EPixelDiffuse");
        material = GetComponent<Renderer>().material;

        if (material.shader == editorDiffuse)
            material.shader = diffuse;
        Shader shader = material.shader;

        positionId = Shader.PropertyToID("_Position");
        cameraPositionId = Shader.PropertyToID("_CameraPosition");
        unitScaleId = Shader.PropertyToID("_UnitScale");

        sceneCamera = GameObject.Find("Scene Camera").transform;

        material.SetFloat(unitScaleId, GraphicsConfig.unitScaleFactor);
    }

    private void LateUpdate()
    {
        material.SetVector(positionId,
            new Vector4(
                transform.position.x,
                transform.position.y,
                transform.position.z,
                0
            )
        );

        material.SetVector(cameraPositionId,
            new Vector4(
                sceneCamera.position.x,
                0,
                sceneCamera.position.z,
                0
            )
        );
    }
}
