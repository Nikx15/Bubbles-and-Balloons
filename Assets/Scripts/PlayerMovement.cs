using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    public bool isKicking = false;
    public bool isJumping = false;

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float kickForce = 20f;
    [SerializeField] private float kickCooldown = 1f; // Cooldown duration in seconds

    private GameObject nearbyBubble = null;
    private bool canKick = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("isJumping", true);
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (Input.GetKeyDown(KeyCode.X) && canKick)
        {
            KickBubble();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        animator.SetFloat("Movement", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("Jump", rb.velocity.y);
    }

    private bool IsGrounded()
    {
        animator.SetBool("isJumping", false);
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void KickBubble()
    {
        if (nearbyBubble != null)
        {
            animator.SetBool("isKicking", true);
            BubbleHandler bubbleHandler = nearbyBubble.GetComponent<BubbleHandler>();
            if (bubbleHandler != null)
            {
                Vector2 kickDirection = isFacingRight ? Vector2.right : Vector2.left;
                bubbleHandler.OnKicked(kickDirection * kickForce);
                StartCoroutine(KickCooldown());
                animator.SetBool("isKicking", false); ;
            }
        }
    }


    private IEnumerator KickCooldown()
    {
        canKick = false;
        yield return new WaitForSeconds(kickCooldown);
        canKick = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            nearbyBubble = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            nearbyBubble = null;
        }
    }
}