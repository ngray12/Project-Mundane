using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class StarEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Spawn Area")]
    public float spawnX = 10f;
    public float verticalRange = 4f;
    public float enemyMoveSpeed = 2f;
    public float spacingX = -1.5f;
 
    [Header("Timing")]
    public float minTimeBetweenWaves = 5f;
    public float maxTimeBetweenWaves = 7f;
    public float postWaveDelay = 2f;

    [Header("Wave Parameters")]
    public int minEnemies = 3;
    public int maxEnemies = 6;
    public float minTimeBetweenEnemies = 0.5f;
    public float maxTimeBetweenEnemies = 0.9f;

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenWaves, maxTimeBetweenWaves);
            yield return new WaitForSeconds(waitTime);

            int patternType = Random.Range(0, 3);
            int count = Random.Range(minEnemies,maxEnemies);
            switch (patternType)
            {
                case 0: yield return StartCoroutine(LineWave(count)); break;
                case 1: yield return StartCoroutine(ArcWave(count)); break;
                case 2: yield return StartCoroutine(SineWave(count)); break;
            }
        }
    }

    IEnumerator LineWave(int count)
    {
        float startY = Random.Range(-verticalRange * 0.5f, verticalRange * 0.5f);
        float endY = Random.Range(-verticalRange * 0.5f, verticalRange * 0.5f);

        for (int i = 0; i < count; i++)
        {
            float t = i / (float)(count - 1);
            float y = Mathf.Lerp(startY, endY, t);
            SpawnEnemy(y);
            yield return new WaitForSeconds(spacingX / enemyMoveSpeed);
        }
    }

    IEnumerator ArcWave(int count)
    {
        float radius = Random.Range(verticalRange * 0.3f, verticalRange * 0.6f);
        float centerY = Random.Range(-verticalRange * 0.3f, verticalRange * 0.3f);
        bool flip = Random.value > 0.5f;

        for (int i = 0; i < count; i++)
        {
            float t = i / (float)(count - 1);
            float angle = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, t);
            float y = centerY + Mathf.Sin(angle) * radius * (flip ? -1f : 1f);
            SpawnEnemy(y);
            yield return new WaitForSeconds(spacingX / enemyMoveSpeed);
        }
    }

    IEnumerator SineWave(int count)
    {
        float amplitude = Random.Range(verticalRange * 0.3f, verticalRange * 0.6f);
        float wavelength = Random.Range(4f, 6f); 
        float centerY = Random.Range(-verticalRange * 0.3f, verticalRange * 0.3f);

        for (int i = 0; i < count; i++)
        {
            float x = spawnX + i * spacingX;              
            float y = centerY + Mathf.Sin((i * spacingX) * (2 * Mathf.PI / wavelength)) * amplitude;
            SpawnEnemyAtPosition(new Vector3(x, y, 0f));
        }

        yield return null; 
    }

    void SpawnEnemyAtPosition(Vector3 position)
    {
        if (!enemyPrefab) return;
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
   
    void SpawnEnemy(float spawnY)
    {
        if (!enemyPrefab) return;
        Vector3 position = new Vector3 (spawnX, spawnY, 0f);
        Instantiate(enemyPrefab, position, Quaternion.identity); 
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
