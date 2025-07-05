using UnityEngine;

/// <summary>
/// Main script for third-person movement of the character using WASD keys.
/// Make sure that the object that receives this script (the player)
/// has the Player tag and a CharacterController component.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonArrow : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Base movement speed (m/s).")]
    public float moveSpeed = 5f;
    [Tooltip("Additional speed when sprinting.")]
    public float sprintAddition = 3.5f;

    [Header("Jump")]
    [Tooltip("Initial jump velocity.")]
    public float jumpForce = 18f;
    [Tooltip("Duration over which jump force falls off.")]
    public float jumpDuration = 0.85f;

    [Header("Gravity")]
    [Tooltip("Gravity acceleration (m/s²).")]
    public float gravity = 9.8f;

    [Header("Turn")]
    [Tooltip("Degrees per second when turning left/right.")]
    public float turnSpeed = 180f;

    private CharacterController cc;
    private Animator animator;

    // Internal state
    private float jumpTimer = 0f;
    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isCrouching = false;

    // Inputs
    private float inputH;
    private float inputV;
    private bool inputJump;
    private bool inputSprint;
    private bool inputCrouch;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("No Animator found—animations won't play.");
    }

    void Update()
    {
        // Read inputs
        inputH = (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f)
               + (Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f);
        inputV = (Input.GetKey(KeyCode.UpArrow) ? 1f : 0f)
               + (Input.GetKey(KeyCode.DownArrow) ? -1f : 0f);

        inputJump = Input.GetKeyDown(KeyCode.RightShift);
        inputSprint = Input.GetKey(KeyCode.RightControl);
        if (Input.GetKeyDown(KeyCode.RightControl))
            isCrouching = !isCrouching;

        // Animations
        if (cc.isGrounded && animator != null)
        {
            animator.SetBool("crouch", isCrouching);
            animator.SetBool("run", cc.velocity.magnitude > 0.9f);
            isSprinting = cc.velocity.magnitude > 0.9f && inputSprint;
            animator.SetBool("sprint", isSprinting);
        }
        if (animator != null)
            animator.SetBool("air", !cc.isGrounded);

        // Initiate jump
        if (inputJump && cc.isGrounded)
            isJumping = true;
    }

    void FixedUpdate()
    {
        // 1) Turn character with A/D
        if (inputH != 0f)
        {
            float turnAmount = inputH * turnSpeed * Time.deltaTime;
            transform.Rotate(0f, turnAmount, 0f);
        }

        // 2) Move forward/back with W/S
        float speedAdd = isSprinting ? sprintAddition
                       : isCrouching ? -moveSpeed * 0.5f
                       : 0f;
        float currentSpeed = moveSpeed + speedAdd;
        Vector3 horizontalMove = transform.forward * (inputV * currentSpeed * Time.deltaTime);

        // 3) Handle jump + gravity
        float verticalMove = 0f;
        if (isJumping)
        {
            verticalMove = Mathf.SmoothStep(jumpForce, jumpForce * 0.3f, jumpTimer / jumpDuration)
                         * Time.deltaTime;
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpDuration)
            {
                isJumping = false;
                jumpTimer = 0f;
            }
        }
        verticalMove -= gravity * Time.deltaTime;

        // 4) Apply movement
        Vector3 motion = horizontalMove + Vector3.up * verticalMove;
        cc.Move(motion);
    }

    // Cancel jump if hitting head
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Vector3.Dot(hit.normal, Vector3.down) > 0.5f && isJumping)
        {
            isJumping = false;
            jumpTimer = 0f;
        }
    }
}
