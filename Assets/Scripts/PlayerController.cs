// Copyright (c) 2020 by Yuya Yoshino

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController Player;

    public float movespeed = 6f;

    private void Awake()
    {
        Player = GetComponentInChildren<CharacterController>();
    }

    private void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            Player.Move(direction * movespeed * Time.deltaTime);
        }
    }
}
