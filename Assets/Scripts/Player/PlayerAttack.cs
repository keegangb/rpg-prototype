// Copyright 2020, Keegan Beaulieu

using UnityEngine;

public class PlayerAttack : Action
{
    private enum AttackPhase
    {
        Delay,
        Strike,
        Recovery
    }

    public float strikeDuration;
    public float delayDuration;
    public float recoveryDuration;

    public float extendLength;
    public float stepForce;

    public GameObject attackPrefab;
    public Damager damager;

    private PhysicsBody physicsBody;
    private Hitbox hitbox;

    private float phaseTimer = 0f;
    private AttackPhase phase;
    private GameObject attackInstance = null;

    public override void OnActionBegin()
    {
        phaseTimer = 0f;
        phase = AttackPhase.Delay;

        actionCancellable = true;

        OrientPlayerWithMouse();
    }

    public override void OnActionCancel()
    {
        DestroyAttackCollider();
    }

    public override void OnActionUpdate()
    {
        switch (phase)
        {
        case AttackPhase.Delay:
            Delay();
            break;
        case AttackPhase.Strike:
            Strike();
            break;
        case AttackPhase.Recovery:
            Recovery();
            break;
        }

        phaseTimer += Time.deltaTime;
    }

    // ----- HELPERS -----
    // {

    private void OrientPlayerWithMouse()
    {
        transform.forward = new Vector3(UserInput.mouseDirection.x, 0,
                                        UserInput.mouseDirection.y);
    }

    private void DestroyAttackCollider()
    {
        if (attackInstance)
            Destroy(attackInstance);

        attackInstance = null;
    }

    private void ApplyStepForce()
    {
        physicsBody.AddForce(transform.forward*stepForce);
    }

    private void InstantiateAttackCollider()
    {
        attackInstance = Instantiate(attackPrefab);
        attackInstance.transform.parent = transform;

        BasicAttackCollider basicAttack = attackInstance.GetComponent<BasicAttackCollider>();
        basicAttack.damager = damager;
        basicAttack.ownerHitbox = hitbox;

        Vector3 pos = transform.position;
        pos += transform.forward*extendLength;

        attackInstance.transform.position = pos;
        attackInstance.transform.rotation = transform.rotation;
    }

    // }
    // ----- HELPERS -----

    private void Delay()
    {
        if (phaseTimer > delayDuration)
        {
            phase = AttackPhase.Strike;
            phaseTimer -= delayDuration;

            actionCancellable = false;

            ApplyStepForce();
            InstantiateAttackCollider();
        }
    }

    private void Strike()
    {
        if (phaseTimer > strikeDuration)
        {
            phase = AttackPhase.Recovery;
            phaseTimer -= strikeDuration;

            DestroyAttackCollider();
        }
    }

    private void Recovery()
    {
        if (phaseTimer > recoveryDuration)
            EndAction();
    }

    protected override void Start()
    {
        base.Start();

        actionExclusive = true;
        actionString = "Attack";
        actionCancelBlacklist.Add("Movement");

        physicsBody = GetComponent<PhysicsBody>();
        hitbox = transform.Find("Hitbox").GetComponent<Hitbox>();
    }

    private void Update()
    {
        if (UserInput.attack)
            RequestAction();
    }
}
