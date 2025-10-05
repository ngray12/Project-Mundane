using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarBoss : MonoBehaviour
{

    [Header("Rotation & Shooting")]
    public float shootInterval = 1.5f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 5f;

    [Header("Arena Bounds")]
    public Vector2 arenaBounds = new Vector2(8f, 4f);

    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;

    private Transform player;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource hitSound;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(ShootAtPlayer());
    }

    void Update() 
    {
        if (player != null)
        {
            RotateTowardsPlayer();
        }
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }


    IEnumerator ShootAtPlayer()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(shootInterval);
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null || shootPoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        PlayShootSound();
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootPoint.up * projectileSpeed; 
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        PLayHitSound();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Star Boss Defeated!");
        Destroy(gameObject);
    }
    void PlayShootSound()
    {
        if (shootSound != null)
        {
            shootSound.Play();
        }
    }

    void PLayHitSound()
    {
        if (hitSound != null)
        {
            hitSound.Play();
        }
    }


    //----------------------GIZMOS-------------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(arenaBounds.x * 2, arenaBounds.y * 2, 1f));

        if (shootPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.up * 2f);
        }
    }
}
