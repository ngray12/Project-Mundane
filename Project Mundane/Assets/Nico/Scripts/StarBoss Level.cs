using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBossLevel : MonoBehaviour
{
    [Header("Target Settings")]
    public GameObject targetPrefab;
    public int maxTargets = 5;
    public float spawnInterval = 3f;

    [Header("Arena Bounds")]
    public Vector2 arenaBounds = new Vector2(8f, 4f);

    private List<GameObject> spawnedTargets = new List<GameObject>();
    private float lastSpawnTime = 0f;

    void Update()
    {
        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            SpawnTarget();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnTarget()
    {
        if (targetPrefab == null) return;

        // Remove oldest if at max
        if (spawnedTargets.Count >= maxTargets)
        {
            Destroy(spawnedTargets[0]);
            spawnedTargets.RemoveAt(0);
        }

        // Spawn at random position within arena
        Vector3 spawnPos = new Vector3(
            Random.Range(-arenaBounds.x, arenaBounds.x),
            Random.Range(-arenaBounds.y, arenaBounds.y),
            0f
        );

        GameObject target = Instantiate(targetPrefab, spawnPos, Quaternion.identity);
        spawnedTargets.Add(target);
    }
}
