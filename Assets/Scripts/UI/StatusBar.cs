using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StatusBar : MonoBehaviour
{
    [Range(0, 100)] public float percent = 100f;

    private Material material;
    private int percentId;

    private RectTransform rect;

    private void Start()
    {
        material = GetComponent<RawImage>().material;
        percentId = Shader.PropertyToID("_Percent");

        material.SetFloat(percentId, percent);
        lastPercent = percent;

        rect = GetComponent<RectTransform>();
    }

    float lastPercent;
    private void Update()
    {
    #if UNITY_EDITOR
        material = GetComponent<RawImage>().material;
        rect = GetComponent<RectTransform>();
    #endif

        if (lastPercent != percent)
        {
            float normalized = percent/100f;
            float sizex = rect.sizeDelta.x;
            float snapped = Mathf.Round(normalized*sizex)/sizex*100f;

            material.SetFloat(percentId, snapped);
        }

        lastPercent = percent;
    }
}