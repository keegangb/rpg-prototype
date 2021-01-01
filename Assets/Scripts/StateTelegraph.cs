using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTelegraph : ActionEventHandler
{
    public Color attackColor;

    private Material material;
    private Color baseColor;

    private void Start()
    {
        Transform graphics = Utils.FindChildRecursive("Graphics", transform);
        material = graphics.GetComponent<MeshRenderer>().sharedMaterial;

        baseColor = material.color;
    }

    protected override void OnActionBegin(string action)
    {
        if (action == "Attack")
            material.color = attackColor;
    }

    protected override void OnActionCancel(string action)
    {
        if (action == "Attack")
            material.color = baseColor;
    }
}
