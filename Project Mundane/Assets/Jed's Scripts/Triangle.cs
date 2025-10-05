using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Triangle : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float jumpBoost = 2f;
    [SerializeField] Transform triangle;

    [Header("Jump Settings")]
    [SerializeField] float maxJumpTime = 0.25f;
    [SerializeField] float jumpHoldForce = 8f;
    [SerializeField] float shortHopMultiplier = 0.5f;

    [Header("Ground Check Offset")]
    [SerializeField] float groundCheckYOffset = -0.2f;

    [Header("Landing Offset")]
    [SerializeField] float landingOffset = -0.3f;

    private bool isJumping = false;
    private float jumpTimeCounter = 0f;
    private bool wasGroundedLastFrame = true;

    public bool isGrounded()
    {
        float checkRadius = 0.4f;
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y + groundCheckYOffset);
        return Physics2D.OverlapCircle(checkPos, checkRadius, groundLayer);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Forward movement
        float newXPos = transform.position.x + speed * Time.deltaTime;
        transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);

        HandleJump();
    }

    void HandleJump()
    {
        bool jumpPressed = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) ||
                           Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space);
        bool jumpHeld = Input.GetMouseButton(0) || Input.GetKey(KeyCode.W) ||
                         Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
        bool jumpReleased = Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.W) ||
                            Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space);

        bool grounded = isGrounded();

        // ----- Jump Start -----
        if (grounded && jumpPressed)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * jumpBoost, ForceMode2D.Impulse);

            isJumping = true;
            jumpTimeCounter = maxJumpTime;
        }

        // ----- Jump Hold -----
        if (isJumping && jumpHeld)
        {
            if (jumpTimeCounter > 0)
            {
                rb.AddForce(Vector2.up * jumpHoldForce * Time.deltaTime, ForceMode2D.Force);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // ----- Short Hop -----
        if (isJumping && jumpReleased)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * shortHopMultiplier);
            }
            isJumping = false;
        }

        // ----- Landing -----
        if (grounded && !wasGroundedLastFrame) // just landed
        {
            // Snap rotation
            Vector3 Rotation = triangle.rotation.eulerAngles;
            Rotation.z = Mathf.Round(Rotation.z / 120) * 120;
            triangle.rotation = Quaternion.Euler(Rotation);

            // Apply backward offset once
            transform.position += new Vector3(landingOffset, 0, 0);
        }
        else if (!grounded)
        {
            // Spin visually while in air
            triangle.Rotate(Vector3.back * 904.83f * Time.deltaTime);
        }

        // Update grounded state for next frame
        wasGroundedLastFrame = grounded;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) 
        {
            if (collision.CompareTag("Death"))
            {
                Destroy(gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    // ------------------ Gizmos ------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        float checkRadius = 0.4f;
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y + groundCheckYOffset);
        Gizmos.DrawWireSphere(checkPos, checkRadius);
    }
}
