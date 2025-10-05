using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    [Header("Spawn Settings")]
    public float spawnX = 10f;             
    public float verticalRange = 4f;       
    public float minTimeBetweenObstacles = 1.2f;
    public float maxTimeBetweenObstacles = 2.5f;

    [Header("Obstacle Settings")]
    public float minGap = 1.5f;            // min distance between top/bottom obstacles
    public float maxGap = 3f;

    public float singleSpawnChance = .8f;



    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float wait = Random.Range(minTimeBetweenObstacles, maxTimeBetweenObstacles);
            yield return new WaitForSeconds(wait);

            if(Random.value < singleSpawnChance)
            {
                SpawnSingleObstacle();
            }
            else
            {
                SpawnObastaclePair();
            }
            
        }
    }

    void SpawnSingleObstacle()
    {
        float y = GetBiasedY();
        GameObject obs = Instantiate(obstaclePrefab, new Vector3(spawnX, y, 0f), Quaternion.identity);
        obs.transform.localScale = new Vector3(1f, Random.Range(minGap, maxGap), 1f);
    }

    void SpawnObastaclePair()
    {
        float gapCenterY = Random.Range(-verticalRange * 0.5f, verticalRange * 0.5f);
        float gapSize = Random.Range(minGap, maxGap);

        // Top obstacle
        float topHeight = verticalRange - (gapCenterY + gapSize / 2f);
        Vector3 topPos = new Vector3(spawnX, gapCenterY + gapSize / 2f + topHeight / 2f, 0f);
        GameObject topObstacle = Instantiate(obstaclePrefab, topPos, Quaternion.identity);
        topObstacle.transform.localScale = new Vector3(1f, topHeight, 1f);

        // Bottom obstacle
        float bottomHeight = verticalRange + (gapCenterY - gapSize / 2f);
        Vector3 bottomPos = new Vector3(spawnX, gapCenterY - gapSize / 2f - bottomHeight / 2f, 0f);
        GameObject bottomObstacle = Instantiate(obstaclePrefab, bottomPos, Quaternion.identity);
        bottomObstacle.transform.localScale = new Vector3(1f, bottomHeight, 1f);
    }

    float GetBiasedY()
    {
        float rand = Random.value;

        if (rand < 0.45f)
        {
            //Top
            return Random.Range(verticalRange * 0.3f, verticalRange);
        }
        else if (rand < 0.9f)
        {
            // Bottom 
            return Random.Range(-verticalRange, -verticalRange * 0.3f);
        }
        else
        {
            // Middle (rare)
            return Random.Range(-verticalRange * 0.3f, verticalRange * 0.3f);
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 top = new Vector3(spawnX, verticalRange, 0f);
        Vector3 bottom = new Vector3(spawnX, -verticalRange, 0f);
        Gizmos.DrawLine(top, bottom);
        Gizmos.DrawWireSphere(top, 0.2f);
        Gizmos.DrawWireSphere(bottom, 0.2f);
    }
}
