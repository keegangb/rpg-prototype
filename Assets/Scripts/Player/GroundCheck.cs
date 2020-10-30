// Copyright 2020, Keegan Beaulieu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool grounded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
            grounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
            grounded = false;
    }
}
