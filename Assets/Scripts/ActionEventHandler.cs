// Copyright 2020, Keegan Beaulieu

using UnityEngine;

public class ActionEventHandler : MonoBehaviour
{
    protected int activeActionCount = 0;

    protected virtual void OnActionBegin(string action) { }
    protected virtual void OnActionCancel(string action) { }

    private void OnActionBeginCallback(string action)
    {
        ++activeActionCount;
        OnActionBegin(action);
    }

    private void OnActionCancelCallback(string action)
    {
        --activeActionCount;
        OnActionBegin(action);
    }

    protected virtual void Awake()
    {
        ActionManager actionManager = GetComponent<ActionManager>();

    #if DEBUG
        if (actionManager == null)
        {
            string error = string.Format(
                "Failed to find an action manager on: {0}.", name
            );

            print(error);
        }
    #endif

        actionManager.onActionBegin += OnActionBegin;
        actionManager.onActionCancel += OnActionCancel;
    }
}
