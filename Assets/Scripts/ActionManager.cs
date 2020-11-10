// Copyright 2020, Keegan Beaulieu

using System.Collections;
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

    public void RequestAction(Action action)
    {
        requestQueue.Enqueue(action);
    }

    private bool TryCancel(Action action, Action toReplace)
    {
        foreach (string conflict in action.cancelBlacklist)
        {
            if (conflict == toReplace.actionString)
                return false;
        }

        action.OnActionCancel();
        activeActions.Remove(action.actionString);
        if (action.exclusive)
            activeExclusive = null;

        return true;
    }

    private void ProcessRequest(Action action)
    {
        if (activeActions.ContainsKey(action.actionString))
            return;

        if (activeExclusive)
        {
            if (activeExclusive.cancellable)
            {
                bool cancelled = TryCancel(activeExclusive, action);
                if (!cancelled)
                    return;
            }
            else
            {
                bool exceptionFound = true;

                if (!exceptionFound)
                    return;
            }
        }
        else
        {
            if (action.exclusive)
            {
                // Queue<Action> exceptionMatches = new Queue<Action>();
                // remove every exception into matches

                foreach (Action activeAction in activeActions.Values)
                {
                    activeAction.OnActionCancel();
                }
                activeActions.Clear();
            }
            else
            {
                List<Conflict> conflicts;
                if (conflictsMap.TryGetValue(action.actionString, out conflicts))
                {
                    foreach (Conflict conflict in conflicts)
                    {
                        if (conflict.other == action.actionString)
                        {

                        }
                    }
                }
            }
        }

        action.OnActionBegin();
        activeActions.Add(action.actionString, action);
        if (action.exclusive)
            activeExclusive = action;
    }

    private void LateUpdate()
    {
        foreach (Action request in requestQueue)
        {
            ProcessRequest(request);
        }
        requestQueue.Clear();

        foreach (Action action in activeActions.Values)
        {
            action.OnActionUpdate();
        }
    }
}
