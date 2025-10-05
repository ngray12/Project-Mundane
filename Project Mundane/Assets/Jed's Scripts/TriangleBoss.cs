using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriangleBoss : MonoBehaviour
{
    [SerializeField] Vector3 topPoint;
    [SerializeField] Vector3 bottomPoint;
    [SerializeField] float speed;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] int posNumber;
    [SerializeField] float posChangeFrequencyTime;
    [SerializeField] float lethalRoundFireRate;
    [SerializeField] float bounceRoundFireRate;
    private float timer;
    private float lethalTimer;
    private float bounceTimer;
    [SerializeField] Transform lethalShotSpawnPoint;
    [SerializeField] Transform bounceShotSpawnPoint;
    public GameObject lethalRound;
    [SerializeField] GameObject bounceRound;
    [SerializeField] float sizeDecrease;
    [SerializeField] bool dead;
    public SpriteRenderer spriteRenderer;
    [SerializeField] float Force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) { return; }
        Move();
        RotationalChanges();
    }

    private void Move()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);

        float newX = Mathf.Lerp(topPoint.x, bottomPoint.x, t);
        float newY = Mathf.Lerp(topPoint.y, bottomPoint.y, t);

        float zOffset = transform.localPosition.z;

        transform.localPosition = new Vector3(newX, newY, zOffset);
    }

    void RotationalChanges() 
    {
        switch (posNumber)
        { 
            case 0:
                transform.eulerAngles = new Vector3(0f, 0f, -150);
                break;
            case 1:
                transform.eulerAngles = new Vector3(0f, 0f, -30f);
                LethalRound();
                break;
            case 2:
                transform.eulerAngles = new Vector3(0f, 0f, -270);
                BounceRound();
                break;
            default:
                transform.eulerAngles = new Vector3(0f, 0f, -150);
                break;
        }
        timer += Time.deltaTime;
        if (timer > posChangeFrequencyTime)
        {
            posNumber++;
            timer = 0;
        }
        if(posNumber > 2) 
        {
            posNumber = 0;
        }
    }

    void LethalRound() 
    {
        if (posNumber == 1)
        {
            lethalTimer += Time.deltaTime;
            if (lethalTimer > lethalRoundFireRate)
            {
                GameObject BulletSpawn = Instantiate(lethalRound, lethalShotSpawnPoint.position, Quaternion.identity);
                BulletSpawn.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Force);
                lethalTimer = 0;
            }
        }
    }
    void BounceRound() 
    {
        if (posNumber == 2)
        {
            bounceTimer += Time.deltaTime;
            if (bounceTimer > bounceRoundFireRate)
            {
                GameObject BulletSpawn = Instantiate(bounceRound, bounceShotSpawnPoint.position, Quaternion.identity);
                BulletSpawn.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Force);
                bounceTimer = 0;
            }
        }
    }
    public void Shrink() 
    {
        transform.localScale -= new Vector3(sizeDecrease, sizeDecrease, sizeDecrease);
        
        if(transform.localScale.x <= 0.3f) 
        {
            lethalShotSpawnPoint.localPosition = new Vector3(-1.46f, -1.96f, 10);
            bounceShotSpawnPoint.localPosition = new Vector3(1,2.74f, 10);
        }
        
        if (transform.localScale.x <= 0.1f) // Example minimum size
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Death();
        }
    }
    void Death()
    {
        dead = true;
        spriteRenderer.color = new Color32(58,56,56,255);
        Debug.Log("Death");
    }
}
