// Copyright 2020, Keegan Beaulieu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfect3D : MonoBehaviour
{
    public static Vector3 SnapPos(Vector3 _pos)
    {
        int scale = GraphicsConfig.unitScaleFactor;

        Vector3 pos = _pos;

        pos.x = Mathf.Round(pos.x*scale)/scale;
        pos.y = Mathf.Round(pos.y*scale)/scale;
        pos.z = Mathf.Round(pos.z*scale)/scale;

        return pos;
    }

    private void Start()
    {
        Shader diffuse = Shader.Find("Custom/PixelDiffuse");
        Shader editorDiffuse = Shader.Find("Custom/EPixelDiffuse");

        Shader surface = Shader.Find("Custom/PixelSurface");
        Shader editorSurface = Shader.Find("Custom/EPixelSurface");

        Shader grass = Shader.Find("Custom/PixelGrass");
        Shader editorGrass = Shader.Find("Custom/EPixelGrass");

        Renderer renderer = GetComponent<Renderer>();
        if (renderer)
        {
            Material material = renderer.material;

            if (material.shader == editorDiffuse)
            {
                float shadowMultiplier = material.GetFloat("_ShadowMultiplier");

                material.shader = diffuse;
                material.SetFloat("_ShadowMultiplier", shadowMultiplier);
            }
            else if (material.shader == editorSurface)
            {
                material.shader = surface;
            }
            else if (material.shader == editorGrass)
            {
                material.shader = grass;
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = SnapPos(transform.position);
    }
}