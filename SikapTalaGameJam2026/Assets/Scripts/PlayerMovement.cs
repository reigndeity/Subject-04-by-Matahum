using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float groundedForce = -2f;

    CharacterController controller;
    float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Move()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = groundedForce;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        move *= moveSpeed;

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        if (verticalVelocity < 0f)
            verticalVelocity += gravity * (fallMultiplier - 1f) * Time.deltaTime;
        else if (verticalVelocity > 0f && !Input.GetButton("Jump"))
            verticalVelocity += gravity * (lowJumpMultiplier - 1f) * Time.deltaTime;

        verticalVelocity += gravity * Time.deltaTime;

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }
}