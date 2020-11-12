// Copyright 2020, Keegan Beaulieu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvent
{
    private enum Type
    {
        Key,
        Mouse
    }

    public static float releaseDelay = 0.1f;

    Type type;

    KeyCode key;
    int mouseButton;

    float timer = 0f;
    bool active = false;

    public InputEvent(KeyCode keyCode)
    {
        type = Type.Key;
        key = keyCode;
    }

    public InputEvent(int _mouseButton)
    {
        type = Type.Mouse;
        mouseButton = _mouseButton;
    }

    private void Listen()
    {
        bool eventFired = false;

        switch (type)
        {
        case Type.Key:
            eventFired = Input.GetKeyDown(key);
            break;
        case Type.Mouse:
            eventFired = Input.GetMouseButtonDown(mouseButton);
            break;
        }

        if (eventFired)
        {
            timer = 0f;
            active = true;
        }
    }

    private void TickTimer()
    {
        if (timer > releaseDelay)
            active = false;

        timer += Time.deltaTime;
    }

    /* 
     * Call this every frame and it will return whether the event is active or not.
     * This class takes into account the static variable release delay in order-
     * to create the effect of an action queue.
     */
    public bool Update()
    {
        Listen();

        if (active)
            TickTimer();

        return active;
    }
}