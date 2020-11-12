// Copyright 2020, Keegan Beaulieu

using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private struct Conflict
    {
        public string other;
        public bool hasPriority;
    }
    
    private Dictionary<string, List<Conflict>> conflictsMap =
            new Dictionary<string, List<Conflict>>();

    private Dictionary<string, Action> activeActions = new Dictionary<string, Action>();
    private Action activeExclusive = null;

    private Queue<Action> requestQueue = new Queue<Action>();

    public void AddConflict(string priorityAction, string otherAction)
    {
        RegisterConflict(priorityAction, otherAction, true);
        RegisterConflict(otherAction, priorityAction, false);
    }

    // For forcing an action to begin
    public void ForceAction(Action action)
    {
        if (action.actionExclusive)
        {
            CancelAll(action.exceptions);
        }
        else
        {
            if (activeExclusive)
            {
                bool exempt = ExceptionPass(action);
                if (!exempt)
                    CancelAction(activeExclusive);
            }

            CancelConflictsOf(action);
        }

        BeginAction(action);
    }

    // For force cancelling an action
    public void CancelAction(Action action)
    {
        if (!IsActive(action))
            return;

        action.OnActionCancel();
        activeActions.Remove(action.actionString);
        if (action.actionExclusive)
            activeExclusive = null;
    }

    public void RequestAction(Action action)
    {
        requestQueue.Enqueue(action);
    }

    // ----- HELPERS -----
    // {

    private bool IsActive(Action action)
    {
        if (activeActions.ContainsKey(action.actionString))
            return true;
        else
            return false;
    }

    private void CancelConflictsOf(Action action)
    {
        List<Conflict> conflicts = GetConflictsOf(action.actionString);
        if (conflicts == null)
            return;

        foreach (Conflict conflict in conflicts)
        {
            Action otherAction;
            if (activeActions.TryGetValue(conflict.other, out otherAction))
                CancelAction(otherAction);
        }
    }

    private void RegisterConflict(string host, string other, bool hostHasPriority)
    {
        Conflict conflict;
        conflict.other = other;
        conflict.hasPriority = hostHasPriority;

        List<Conflict> conflicts = GetConflictsOf(host);
        if (conflicts == null)
        {
            conflicts = CreateConflictsFor(host);
        }
        else
        {
            foreach (Conflict existingConflict in conflicts)
            {
                if (existingConflict.other == other)
                    return;
            }
        }

        conflicts.Add(conflict);
    }

    // Try to cancel an exclusive action
    private bool TryCancelActiveExclusiveWith(Action toReplace)
    {
        if (!activeExclusive.actionCancellable)
            return false;

        foreach (string conflict in activeExclusive.actionCancelBlacklist)
        {
            if (conflict == toReplace.actionString)
                return false;
        }

        CancelAction(activeExclusive);

        return true;
    }

    private bool ExceptionPass(Action action)
    {
        List<string> exceptions = activeExclusive.exceptions;
        foreach (string exception in exceptions)
        {
            if (exception == action.actionString)
                return true;
        }

        return false;
    }

    /*
     * Does a pass in attempt to resolve conflicts so that the action may begin.
     * Returns whether or not conflicts could be resolved for action.
     */
    private bool ConflictPass(Action action)
    {
        List<Conflict> conflicts = GetConflictsOf(action.actionString);
        if (conflicts == null)
            return true;

        Queue<Action> activesToRemove = new Queue<Action>();
        foreach (Conflict conflict in conflicts)
        {
            Action conflictAction;
            if (activeActions.TryGetValue(conflict.other, out conflictAction))
            {
                if (conflict.hasPriority)
                    activesToRemove.Enqueue(conflictAction);
                else
                    return false;
            }
        }

        foreach (Action activeConflict in activesToRemove)
        {
            CancelAction(activeConflict);
        }

        return true;
    }

    private List<Conflict> GetConflictsOf(string action)
    {
        List<Conflict> conflicts = null;
        conflictsMap.TryGetValue(action, out conflicts);

        return conflicts;
    }

    private List<Conflict> CreateConflictsFor(string action)
    {
        List<Conflict> conflicts = new List<Conflict>();
        conflictsMap.Add(action, conflicts);

        return conflicts;
    }

    private void CancelAll(List<string> exceptions)
    {
        List<Action> activeExceptions = new List<Action>();
        foreach (string exception in exceptions)
        {
            Action exceptionAction;
            if (activeActions.TryGetValue(exception, out exceptionAction))
                activeExceptions.Add(exceptionAction);
        }

        foreach (Action action in activeActions.Values)
        {
            action.OnActionCancel();
        }

        activeActions.Clear();
        activeExclusive = null;

        foreach (Action exception in activeExceptions)
        {
            activeActions.Add(exception.actionString, exception);
        }
    }

    private void BeginAction(Action action)
    {
        action.OnActionBegin();
        activeActions.Add(action.actionString, action);
        if (action.actionExclusive)
            activeExclusive = action;
    }

    // CancelAction(Action) defined in publics

    // }
    // --- HELPERS ---

    private void ProcessExclusiveRequest(Action action)
    {
        if (activeExclusive)
        {
            if (!TryCancelActiveExclusiveWith(action))
                return;
        }

        CancelAll(action.exceptions);
        BeginAction(action);
    }

    private void ProcessNonExclusiveRequest(Action action)
    {
        if (activeExclusive)
        {
            bool exempt = ExceptionPass(action);

            if (!exempt)
            {
                bool cancelled = TryCancelActiveExclusiveWith(action);
                if (!cancelled)
                    return;
            }
        }

        bool passed = ConflictPass(action);
        if (!passed)
            return;

        BeginAction(action);
    }

    private void ProcessRequest(Action action)
    {
        if (IsActive(action))
            return;

        if (action.actionExclusive)
            ProcessExclusiveRequest(action);
        else
            ProcessNonExclusiveRequest(action);
    }

    private void LateUpdate()
    {
        foreach (Action request in requestQueue)
        {
            ProcessRequest(request);
        }
        requestQueue.Clear();

        List<Action> actionList = new List<Action>(activeActions.Values);
        foreach (Action action in actionList)
        {
            action.OnActionUpdate();
        }
    }
}
