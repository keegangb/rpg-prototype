using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    public Damager damager;

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
        if (hitbox)
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

    private void Update()
    {
    #if UNITY_EDITOR
        Vector3 size = Vector3.one;

        System.Type type = _collider.GetType();
        if (type == typeof(BoxCollider))
            size = ((BoxCollider)(_collider)).size;

        SceneDebugger.DrawMesh(debugMesh, debugMeshMaterial, transform.position,
                               transform.rotation, size);
    #endif
    }
}
