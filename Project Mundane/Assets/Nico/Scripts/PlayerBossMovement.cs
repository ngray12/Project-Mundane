using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBossMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Arena Bounds")]
    public Vector2 arenaBounds = new Vector2(8f, 4f);

    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;
    public float spawnInterval = 1f;   
    public int maxObstacles = 5;
    public Vector2 spawnOffset = new Vector2(1.5f, 0f);

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float lastSpawnTime = 0f;
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); 
        float vertical = Input.GetAxisRaw("Vertical");     
        moveInput = new Vector2(horizontal, vertical).normalized;

        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            SpawnObstacle();
            lastSpawnTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        // Move player
        rb.velocity = moveInput * moveSpeed;

        // Clamp position inside arena
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -arenaBounds.x, arenaBounds.x);
        pos.y = Mathf.Clamp(pos.y, -arenaBounds.y, arenaBounds.y);
        transform.position = pos;
    }

    void SpawnObstacle()
    {
        //if (obstaclePrefab = null) return;

        if (spawnedObstacles.Count >= maxObstacles)
        {
            Destroy(spawnedObstacles[0]);
            spawnedObstacles.RemoveAt(0);
        }

        Vector3 spawnPosition = transform.position + (Vector3)spawnOffset;

        spawnPosition.x = Mathf.Clamp(spawnPosition.x, -arenaBounds.x, arenaBounds.x);
        spawnPosition.y = Mathf.Clamp(spawnPosition.y, -arenaBounds.y, arenaBounds.y);

        GameObject obstacle = Instantiate(obstaclePrefab,spawnPosition,Quaternion.identity);
        spawnedObstacles.Add(obstacle);

    }   
}
