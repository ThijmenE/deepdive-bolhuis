using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class MovementScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7.5f;
    public float jumpTime = 0f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    [Header("Wall Jump Settings")]
    public Vector2 wallJumpingPower = new Vector2(2f, 7.5f);
    public float wallSlidingSpeed = 2f;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingDuration = 0.4f;
    public float wallJumpLockTime = 0.1f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    private float jumpTimeCounter;
    public int currentJumpCount = 0;
    public int maxJumpCount = 1;

    private bool isFacingRight = true;
    public Animator animator;

    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private float wallJumpLockCounter;

    [SerializeField] private Transform wallCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (wallJumpLockCounter > 0f)
        {
            wallJumpLockCounter -= Time.deltaTime;
            return;
        }

        float moveInput = 0f;
        float deadzone = 0.5f;
        if (Input.GetKey(KeyCode.A)) moveInput = -1f;
        else if (Input.GetKey(KeyCode.D)) moveInput = 1f;
        else
        {
            float axisInput = Input.GetAxis("Horizontal");
            moveInput = Mathf.Abs(axisInput) < deadzone ? 0f : axisInput;
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        if (moveInput > 0 && !isFacingRight) Flip();
        else if (moveInput < 0 && isFacingRight) Flip();

        isGrounded = Physics2D.
            BoxCast(boxCollider.bounds.center, boxCollider.bounds.size * new Vector2(0.9f, 1f),
                    0f, Vector2.down, 0.2f, groundLayer);

        if (isGrounded)
        {
            currentJumpCount = 0;
            isJumping = false;
            isFalling = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount && !isWallSliding)
        {
            Jump();
            isJumping = true;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            isJumping = false;
        }

        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            isFalling = true;
            isJumping = false;
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }

        WallSlide(moveInput);
        WallJump();
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        isJumping = true;
        jumpTimeCounter = jumpTime;

        currentJumpCount++;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void WallSlide(float moveInput)
    {
        if (IsWalled() && !isGrounded && moveInput != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,
            Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
            animator.SetBool("isWallSliding", true);
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("isWallSliding", false);
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;

            rb.linearVelocity = new Vector2(
                wallJumpingDirection * wallJumpingPower.x,
                wallJumpingPower.y);

            wallJumpingCounter = 0f;

            wallJumpLockCounter = wallJumpLockTime;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
}
