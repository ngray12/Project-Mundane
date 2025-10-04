using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
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
            if (collision.CompareTag("Bullet"))
            {
                Debug.Log("Hit Bullet");
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }

            if (collision.CompareTag("Death"))
            {
                Debug.Log("Hit KillZone");
                Destroy(gameObject);
            }
        }
    }

}
