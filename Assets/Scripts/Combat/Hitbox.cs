using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Health health = null;
    private PhysicsBody physicsBody = null;

    public void TakeHit(Damager damager, Vector3 knockBackDir)
    {
        health.TakeDamage(damager.damage);

        if (physicsBody && damager.force > float.Epsilon)
            physicsBody.AddForce(damager.force*knockBackDir);
    }

    private void Start()
    {
        health = transform.parent.GetComponent<Health>();
        physicsBody = GetComponent<PhysicsBody>();
    }
}