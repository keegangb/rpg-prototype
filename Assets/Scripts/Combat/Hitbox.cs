﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Hitbox : MonoBehaviour
{
    private Health health = null;
    private PhysicsBody physicsBody = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            health.TakeDamage(10f);
        }
    }

    public void TakeHit(Damager damager)
    {
        health.TakeDamage(damager.damage);

        if (physicsBody)
            physicsBody.AddForce(damager.force*damager.transform.forward);
    }

    private void Start()
    {
        health = GetComponent<Health>();
        physicsBody = GetComponent<PhysicsBody>();
    }
}