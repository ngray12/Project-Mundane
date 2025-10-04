using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SquareBoss : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public Vector2 arenaBounds = new Vector2(8f, 4f);
    public float pauseTime = 0.5f;
    public float nudgeDistance = 0.1f;
    public float checkRadius = 0.2f;
    public Transform player;
    [Range(0f, 1f)] public float targetChance = 0.2f;

    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private bool isDead = false;
    private bool isMoving = false;
    private bool lastMoveWasHorizontal;

    private readonly Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentHealth = maxHealth;
        PickNewDirectionInternal(true);
    }

    void FixedUpdate()
    {
        if (isDead || !isMoving) return;

        rb.velocity = moveDir * moveSpeed;

        // Keep inside arena bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -arenaBounds.x, arenaBounds.x);
        pos.y = Mathf.Clamp(pos.y, -arenaBounds.y, arenaBounds.y);
        transform.position = pos;
    }

    void PickNewDirectionInternal(bool start = false)
    {
        if (isDead) return;

        // Tiny nudge away from walls to avoid getting stuck
        transform.position += (Vector3)(-moveDir.normalized * nudgeDistance);

        List<Vector2> validDirs = new List<Vector2>();
        bool willTargetPlayer = player != null && Random.value < targetChance;

        // If targeting the player, pick a valid axis toward them
        if (willTargetPlayer)
        {
            if (lastMoveWasHorizontal) // vertical move next
            {
                if (player.position.y > transform.position.y) validDirs.Add(Vector2.up);
                else if (player.position.y < transform.position.y) validDirs.Add(Vector2.down);
            }
            else // horizontal move next
            {
                if (player.position.x > transform.position.x) validDirs.Add(Vector2.right);
                else if (player.position.x < transform.position.x) validDirs.Add(Vector2.left);
            }
        }

        // If not targeting or no valid axis, pick random along perpendicular axis
        if (validDirs.Count == 0)
        {
            foreach (var dir in directions)
            {
                bool correctAxis = start || (lastMoveWasHorizontal && dir.y != 0) || (!lastMoveWasHorizontal && dir.x != 0);
                if (!correctAxis) continue;

                Vector2 checkPos = (Vector2)transform.position + dir * checkRadius;
                Collider2D hit = Physics2D.OverlapCircle(checkPos, checkRadius, LayerMask.GetMask("ArenaWall"));
                if (hit == null)
                    validDirs.Add(dir);
            }
        }

        // Fallback: pick any direction that keeps inside arena
        if (validDirs.Count == 0)
        {
            foreach (var dir in directions)
            {
                Vector2 checkPos = (Vector2)transform.position + dir * checkRadius;
                Collider2D hit = Physics2D.OverlapCircle(checkPos, checkRadius, LayerMask.GetMask("ArenaWall"));
                if (hit == null)
                    validDirs.Add(dir);
            }
        }

        // Last resort: reverse
        if (validDirs.Count == 0)
            validDirs.Add(-moveDir);

        moveDir = validDirs[Random.Range(0, validDirs.Count)];
        lastMoveWasHorizontal = moveDir.x != 0;
        isMoving = true;
    }

    void PickNewDirection() => PickNewDirectionInternal();

    void StopMoving()
    {
        isMoving = false;
        rb.velocity = Vector2.zero;
        Invoke(nameof(PickNewDirection), pauseTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit Wall");
            StopMoving();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Hit Obstacle");
            TakeDamage();
            StopMoving();
            Destroy(other.gameObject);
        }
    }

        void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
            Die();
        Debug.Log(currentHealth.ToString());
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        Debug.Log("Boss Defeated");
        Destroy(gameObject);
        Invoke(nameof(EndFight), 2f);
    }

    void EndFight() => SceneManager.LoadScene("VictoryScene");

    // ------------------ Gizmos ------------------
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        // Arena bounds
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(arenaBounds.x * 2, arenaBounds.y * 2, 1f));

        // Show check circles for each direction
        Gizmos.color = Color.cyan;
        foreach (var dir in directions)
        {
            Vector2 checkPos = (Vector2)transform.position + dir * checkRadius;
            Gizmos.DrawWireSphere(checkPos, checkRadius);
        }

        // Current move direction
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(moveDir.normalized * 1f));
    }
}
