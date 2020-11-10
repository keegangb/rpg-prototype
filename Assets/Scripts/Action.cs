// Copyright 2020, Keegan Beaulieu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [System.NonSerialized] public string actionString = null;
    [System.NonSerialized] public bool exclusive = true;
    [System.NonSerialized] public bool cancellable = false;
    [System.NonSerialized] public List<string> cancelBlacklist = new List<string>();

    private ActionManager manager;

    public virtual void OnActionBegin() { }
    public virtual void OnActionCancel() { }
    public virtual void OnActionUpdate() { }

    protected virtual void Start()
    {
        manager = GetComponent<ActionManager>();
    }

    protected void EndAction()
    {

    }

    protected void RequestAction()
    {
    #if DEBUG
        if (actionString == null)
        {
            string error = string.Format(
                "RequestAction() failed.\n" +
                "Action {0} was not initialized.\n" +
                "Assign member actionString.",
                this.GetType().Name
            );
            Debug.LogError(error);

            return;
        }
    #endif // DEBUG

        manager.RequestAction(this);
    }
}
