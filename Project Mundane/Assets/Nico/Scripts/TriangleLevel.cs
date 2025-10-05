using System.Collections;
using UnityEngine;

public class TriangleLevel : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] GameObject platformPrefab;
    [SerializeField] float spawnX = 15f;
    [SerializeField] float groundY = -2f;
    [SerializeField] float minSpacing = 1.5f;
    [SerializeField] float maxSpacing = 4f;
    [SerializeField] float linkedGap = 1f;
    [SerializeField] float minWidth = 2f;
    [SerializeField] float maxWidth = 6f;
    [SerializeField] float platformSpeed = 5f;
    [SerializeField][Range(0f, 1f)] float linkChance = 0.3f;
    [SerializeField] float linkedHeightStep = 2f; // BIG increase for jumps
    [SerializeField] int maxLinkedChain = 3;
    [SerializeField] float minHeight = 0.5f; // independent platforms
    [SerializeField] float maxHeight = 1.5f;

    private GameObject lastPlatform = null;

    void Start()
    {
        StartCoroutine(SpawnPlatforms());
    }

    IEnumerator SpawnPlatforms()
    {
        while (true)
        {
            int chainLength = 1;

            if (lastPlatform != null && Random.value < linkChance)
            {
                chainLength = Random.Range(2, maxLinkedChain + 1);
            }

            for (int i = 0; i < chainLength; i++)
            {
                GameObject platform = Instantiate(platformPrefab);

                float width = Random.Range(minWidth, maxWidth);

                float height;
                if (i > 0)
                {
                    // Linked platform: taller for jumping
                    height = linkedHeightStep * i;
                }
                else
                {
                    // Independent platform: random height
                    height = Random.Range(minHeight, maxHeight);
                }

                platform.transform.localScale = new Vector3(width, height, platform.transform.localScale.z);

                float xPos = lastPlatform != null ? lastPlatform.transform.position.x + (lastPlatform.transform.localScale.x / 2f) + width / 2f : spawnX;

                if (i > 0)
                    xPos += linkedGap; // close spacing for linked platforms
                else if (lastPlatform != null)
                    xPos += Random.Range(minSpacing, maxSpacing);

                float yPos = groundY + height / 2f; // bottom stays on ground

                platform.transform.position = new Vector3(xPos, yPos, 0f);

                MovingPlatform mp = platform.GetComponent<MovingPlatform>();
                if (mp == null) mp = platform.AddComponent<MovingPlatform>();
                mp.SetSpeed(platformSpeed);

                lastPlatform = platform;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 pos = new Vector3(spawnX, groundY, 0f);
        Gizmos.DrawSphere(pos, 0.5f);
        Gizmos.DrawLine(pos, pos + Vector3.right * 2f);
    }
}
