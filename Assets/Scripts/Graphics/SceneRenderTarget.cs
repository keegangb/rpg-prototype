// Copyright 2020, Keegan Beaulieu

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SceneRenderTarget : MonoBehaviour
{
    private int screenWidth;
    private int screenHeight;

    private RawImage targetImage;
    private RectTransform targetImageTransform;
    private Camera sceneCamera;

    private void NewTarget()
    {
        float newWidthf = (float)Screen.width/GraphicsConfig.pixelScaleFactor;
        float newHeightf = (float)Screen.height/GraphicsConfig.pixelScaleFactor;

        int newWidth = (int)Mathf.Ceil(newWidthf);
        int newHeight = (int)Mathf.Ceil(newHeightf);

        newWidth += (newWidth % 2 != 0) ? 1 : 0;
        newHeight += (newHeight % 2 != 0) ? 1 : 0;

        RenderTexture targetTexture = new RenderTexture(newWidth, newHeight, 24);
        targetTexture.filterMode = FilterMode.Point;

        targetImage.texture = targetTexture;
        targetImageTransform.sizeDelta = new Vector3(newWidth, newHeight);

        sceneCamera.targetTexture = targetTexture;
        sceneCamera.rect = new Rect(0, 0, newWidth, newHeight);
        sceneCamera.orthographicSize = (float)newHeight/GraphicsConfig.unitScaleFactor/2.0f;
    }

    private void Start()
    {
        GameObject canvas = GameObject.Find("Render Target Canvas");
        GameObject sceneUi = canvas.transform.Find("Scene").gameObject;

        canvas.GetComponent<Canvas>().scaleFactor = GraphicsConfig.pixelScaleFactor;

        targetImage = sceneUi.GetComponent<RawImage>();
        targetImageTransform = sceneUi.GetComponent<RectTransform>();

        sceneCamera = GameObject.Find("Scene Camera").GetComponent<Camera>();

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        NewTarget();
    }

    private void Update()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
            NewTarget();

        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
}
