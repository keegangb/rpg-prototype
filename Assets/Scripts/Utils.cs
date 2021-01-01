// Copyright 2020, Keegan Beaulieu

using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Transform FindChildRecursive(string name, Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }
            else
            {
                Transform matched = FindChildRecursive(name, child);
                if (matched)
                    return matched;
            }
        }

        return null;
    }
}
