using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareObstacle : MonoBehaviour
{
    public float moveSpeed = 3f;

    private void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Hit Player: You Lose");
                
            }

            if (collision.CompareTag("Death"))
            {
                Debug.Log("Hit KillZone");
                Destroy(gameObject);
            }
        }
    }
}
