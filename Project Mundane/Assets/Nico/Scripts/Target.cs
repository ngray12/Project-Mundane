using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int health = 1;
    public StarBoss boss;

    // Start is called before the first frame update
    void Awake()
    {
        boss = FindObjectOfType<StarBoss>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Hit by Bullet");
            TakeDamage();
            Destroy(other.gameObject);
        }
    }

    void TakeDamage()
    {
        health--;

        // Notify boss
        if (boss != null)
        {
            boss.TakeDamage();
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
