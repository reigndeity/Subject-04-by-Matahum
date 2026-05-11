using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float groundedForce = -2f;

    private CharacterController controller;
    private PlayerAnimation anim;

    private float verticalVelocity;

    private bool wasGrounded;
    private bool isFalling;
    private bool isJumping;
    private bool isLanding;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<PlayerAnimation>();
    }

    public void Move()
    {
        bool grounded = controller.isGrounded;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move =
            (transform.right * x + transform.forward * z).normalized;

        bool isMoving = move.magnitude > 0.1f;

        // Horizontal movement
        move *= moveSpeed;

        // Landing detection
        if (grounded && !wasGrounded && isFalling)
        {
            if (!isMoving)
            {
                anim.PlayLandingAnim();
                isLanding = true;

                Invoke(nameof(EndLanding), 0.3f);
            }

            isFalling = false;
        }

        if (grounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedForce;
        }

        // Jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            Jump();
        }

        // Better jump physics
        if (verticalVelocity < 0f)
        {
            if (!grounded)
            {
                Fall();
            }

            verticalVelocity +=
                gravity * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (verticalVelocity > 0f &&
                 !Input.GetButton("Jump"))
        {
            verticalVelocity +=
                gravity * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }

        verticalVelocity += gravity * Time.deltaTime;

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        // Ground animations
        if (grounded && !isFalling && !isJumping && !isLanding)
        {
            if (isMoving)
            {
                anim.PlayJogAnim();
            }
            else
            {
                anim.PlayIdleAnim();
            }
        }

        wasGrounded = grounded;
    }

    void Jump()
    {
        anim.PlayJumpAnim();

        isJumping = true;

        verticalVelocity =
            Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void Fall()
    {
        if (isFalling)
            return;

        anim.PlayFallAnim();
        isJumping = false;
        isFalling = true;
    }

    void EndLanding()
    {
        isLanding = false;
    }
}
