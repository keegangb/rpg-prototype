using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int damage;
    public float force;

    private List<Hitbox> blacklist = new List<Hitbox>();

    private void OnTriggerEnter(Collider other)
    {
        Hitbox hitbox = other.GetComponent<Hitbox>();
        if (hitbox)
        {
            foreach (Hitbox _hitbox in blacklist)
            {
                if (hitbox == _hitbox)
                    return;
            }

            hitbox.TakeHit(this);
            blacklist.Add(hitbox);
        }
    }
}