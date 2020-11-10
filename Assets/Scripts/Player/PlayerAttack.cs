// Copyright 2020, Keegan Beaulieu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // private enum AttackPhase
    // {
    //     Inactive,
    //     Delay,
    //     Strike,
    //     Recovery
    // }

    // public float strikeDuration;
    // public float delayDuration;
    // public float recoveryDuration;

    // public float extendLength;
    // public float stepForce;

    // public GameObject attackPrefab;
    // public Damager damager;

    // private ActionState state;
    // private PhysicsBody physicsBody;

    // private float phaseTimer = 0f;
    // private AttackPhase phase = AttackPhase.Inactive;
    // private GameObject attackInstance = null;

    // private List<string> cancelBlacklist = new List<string>();

    // private void Start()
    // {
    //     state = PlayerActionState.state;
    //     physicsBody = GetComponent<PhysicsBody>();

    //     cancelBlacklist.Add("Movement");
    // }

    // private void ApplyStepForce()
    // {
    //     physicsBody.AddForce(transform.forward*stepForce);
    // }

    // private void InstantiateAttackCollider()
    // {
    //     attackInstance = Instantiate(attackPrefab);
    //     attackInstance.transform.parent = transform;

    //     BasicAttack basicAttack = attackInstance.GetComponent<BasicAttack>();
    //     basicAttack.damager = damager;

    //     Vector3 pos = transform.position;
    //     pos += transform.forward*extendLength;

    //     attackInstance.transform.position = pos;
    //     attackInstance.transform.rotation = transform.rotation;
    // }

    // private void DestroyAttackCollider()
    // {
    //     if (attackInstance)
    //         Destroy(attackInstance);

    //     attackInstance = null;
    // }

    // private void OrientPlayerWithMouse()
    // {
    //     transform.forward = new Vector3(UserInput.mouseDirection.x, 0,
    //                                     UserInput.mouseDirection.y);
    // }
    
    // private void OnCancel()
    // {
    //     phase = AttackPhase.Inactive;
    // }

    // private void Inactive()
    // {
    //     if (UserInput.attack)
    //     {
    //         bool canAttack = state.RequestPrimaryAction("Attack");

    //         if (canAttack)
    //         {
    //             phaseTimer = 0f;
    //             phase = AttackPhase.Delay;

    //             state.EnableCancel("Attack", OnCancel, cancelBlacklist);

    //             OrientPlayerWithMouse();
    //         }
    //     }
    // }

    // private void Delay()
    // {
    //     if (phaseTimer > delayDuration)
    //     {
    //         phase = AttackPhase.Strike;
    //         phaseTimer -= delayDuration;

    //         state.DisableCancel("Attack");

    //         ApplyStepForce();
    //         InstantiateAttackCollider();
    //     }
    // }

    // private void Strike()
    // {
    //     if (phaseTimer > strikeDuration)
    //     {
    //         phase = AttackPhase.Recovery;
    //         phaseTimer -= strikeDuration;

    //         DestroyAttackCollider();
    //     }
    // }

    // private void Recovery()
    // {
    //     if (phaseTimer > recoveryDuration)
    //     {
    //         state.CancelPrimaryAction("Attack");
    //         phase = AttackPhase.Inactive;
    //     }
    // }

    // private void Update()
    // {
    //     switch (phase)
    //     {
    //     case AttackPhase.Inactive:
    //         Inactive();
    //         break;
    //     case AttackPhase.Delay:
    //         Delay();
    //         break;
    //     case AttackPhase.Strike:
    //         Strike();
    //         break;
    //     case AttackPhase.Recovery:
    //         Recovery();
    //         break;
    //     }

    //     if (phase != AttackPhase.Inactive)
    //         phaseTimer += Time.deltaTime;
    // }
}