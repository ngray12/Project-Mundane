using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarPlayerBoss : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Arena Bounds")]
    public Vector2 arenaBounds = new Vector2(8f, 4f);


    private Rigidbody2D rb;
    private Vector2 moveInput;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;
    }

    private void FixedUpdate()
    {
        // Move player
        rb.velocity = moveInput * moveSpeed;

        // Clamp position inside arena
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -arenaBounds.x, arenaBounds.x);
        pos.y = Mathf.Clamp(pos.y, -arenaBounds.y, arenaBounds.y);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log("Hit by Bullet");
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
