// Copyright 2020, Keegan Beaulieu

using UnityEngine;

public class PlayerAnimation : ActionEventHandler
{
    private Animator animator;

    protected override void OnActionBegin(string action)
    {
        animator.SetBool("Idle", false);

        switch (action)
        {
        case "Movement":
            animator.SetBool("Movement", true);
            break;
        case "Attack":
            animator.SetBool("Attack", true);
            break;
        }
    }

    protected override void OnActionCancel(string action)
    {
        if (activeActionCount == 0)
            animator.SetBool("Idle", true);

        switch (action)
        {
        case "Movement":
            animator.SetBool("Movement", false);
            break;
        case "Attack":
            animator.SetBool("Attack", false);
            break;
        }
    }

    private void Start()
    {
        Transform graphics = Utils.FindChildRecursive("Graphics", transform);
        if (graphics)
            animator = graphics.GetComponent<Animator>();

    #if DEBUG
        if (!animator)
        {
            string error = string.Format(
                "Failed to find graphics object for: {0}.", name
            );

            print(error);
        }
    #endif
    }
}
