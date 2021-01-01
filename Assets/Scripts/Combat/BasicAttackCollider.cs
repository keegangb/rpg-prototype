﻿using System.Collections.Generic;
using UnityEngine;

public class BasicAttackCollider : MonoBehaviour
{
    [HideInInspector] public Damager damager;
    [HideInInspector] public Hitbox ownerHitbox = null;

    #if UNITY_EDITOR
    public Mesh debugMesh;
    public Material debugMeshMaterial;
    #endif

    private List<Hitbox> blacklist = new List<Hitbox>();

    #if UNITY_EDITOR
    private Collider _collider;
    #endif

    private void Start()
    {
    #if UNITY_EDITOR
        _collider = GetComponent<Collider>();
    #endif
    }

    private void OnTriggerEnter(Collider other)
    {
        Hitbox hitbox = other.GetComponent<Hitbox>();
        if (hitbox && hitbox != ownerHitbox)
        {
            foreach (Hitbox _hitbox in blacklist)
            {
                if (hitbox == _hitbox)
                    return;
            }

            hitbox.TakeHit(damager, transform.forward);
            blacklist.Add(hitbox);
        }
    }
}