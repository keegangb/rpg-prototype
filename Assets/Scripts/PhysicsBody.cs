using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
    public Vector3 velocity = new Vector3();

    private CharacterController character = null;

    public void AddForce(Vector3 force)
    {
        velocity += force;
    }

    private void Start()
    {
        character = GetComponent<CharacterController>();
    }

    private void Gravity()
    {
        velocity += Physics.gravity*Time.deltaTime;
    }

    private void Move()
    {
        if (character)
            character.Move(velocity*Time.deltaTime);
        else
            transform.position += velocity;
    }

    private void GroundCheck()
    {
        if (character)
        {
            if (character.isGrounded)
                velocity.y = 0f;
        }
    }

    private void LateUpdate()
    {
        Gravity();
        Move();
        GroundCheck();
    }
}
