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

    public Vector3 mouseDirectionWorldBias;

    private PhysicsBody physicsBody;
    private Hitbox hitbox;
    private SmoothRotation smoothRotation;
    private Transform cameraTransform;
    private Camera mainCamera;

    private float phaseTimer = 0f;
    private AttackPhase phase;
    private GameObject attackInstance = null;

    public override void OnActionBegin()
    {
        phaseTimer = 0f;
        phase = AttackPhase.Delay;

        actionCancellable = true;

        smoothRotation.targetDirection = GetMouseDirection();
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
            CancelAction();
    }

    protected override void Start()
    {
        base.Start();

        actionExclusive = true;
        actionString = "Attack";
        actionCancelBlacklist.Add("Movement");

        physicsBody = GetComponent<PhysicsBody>();
        smoothRotation = GetComponent<SmoothRotation>();
        hitbox = transform.Find("Hitbox").GetComponent<Hitbox>();

        cameraTransform = GameObject.Find("Main Camera").transform;
        mainCamera = cameraTransform.GetComponent<Camera>();
    }

    private void Update()
    {
        if (UserInput.attack)
            RequestAction();
    }

    // ----- HELPERS -----
    private Vector3 GetMouseDirection()
    {
        Vector3 worldPosition = transform.position + mouseDirectionWorldBias;

        Vector3 mousePosition;
        mousePosition.x = UserInput.mousePosition.x;
        mousePosition.z = UserInput.mousePosition.y;

        Vector3 playerScreen = mainCamera.WorldToScreenPoint(worldPosition);
        playerScreen.z = playerScreen.y;

        mousePosition.y = 0;
        playerScreen.y = 0;

        Vector3 euler = cameraTransform.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        Quaternion rotationMatrix = Quaternion.Euler(euler);

        return rotationMatrix*(mousePosition - playerScreen);
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
}
