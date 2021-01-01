// Copyright 2020, Keegan Beaulieu

using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [System.NonSerialized] public string actionString = null;
    [System.NonSerialized] public bool actionExclusive = true;
    [System.NonSerialized] public bool actionCancellable = false;
    [System.NonSerialized] public List<string> actionCancelBlacklist = new List<string>();
    [System.NonSerialized] public List<string> exceptions = new List<string>();

    private ActionManager manager;

    public virtual void OnActionBegin() { }
    public virtual void OnActionCancel() { }
    public virtual void OnActionUpdate() { }

    protected void RequestAction()
    {
        if (!ValidateRequest())
            return;

        manager.RequestAction(this);
    }

    protected void ForceAction()
    {
        if (!ValidateRequest())
            return;

        manager.ForceAction(this);
    }

    protected void CancelAction()
    {
        manager.CancelAction(this);
    }

    protected void AddConflict(string priorityAction, string otherAction)
    {
        manager.AddConflict(priorityAction, otherAction);
    }

    protected void AddException(string exception)
    {
        exceptions.Add(exception);
    }

    // ----- HELPERS ------
    private bool ValidateRequest()
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

            return false;
        }

        return true;
    #endif // DEBUG
    }
    // ----- HELPERS ------

    protected virtual void Start()
    {
        manager = GetComponent<ActionManager>();
    }

    protected virtual void OnDestroy()
    {
        manager.CancelAction(this);
    }
}
