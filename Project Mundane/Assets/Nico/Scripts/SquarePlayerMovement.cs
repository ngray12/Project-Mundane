using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SquarePlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float gravityStrength = 20f;

    private Rigidbody2D rb;
    private int gravityDirection = -1;

   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gravityDirection = 1; // gravity pulls upward
            rb.velocity = new Vector2(rb.velocity.x, 0f); // reset vertical momentum
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gravityDirection = -1; // gravity pulls downward
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    
    void FixedUpdate()
    {
        float verticalVelocity = rb.velocity.y + gravityStrength * gravityDirection * Time.fixedDeltaTime;
        rb.velocity = new Vector2(0f, verticalVelocity);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Hit Obstacle");
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
