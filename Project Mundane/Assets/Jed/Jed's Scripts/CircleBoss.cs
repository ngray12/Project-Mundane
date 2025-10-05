using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CircleBoss : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Transform player;
    [SerializeField] int attackNumber;
    [SerializeField] int increaseAmount;
    [SerializeField] int currentAttackNumber;
    [SerializeField] int speed;
    [SerializeField] float timeBtwAttack;
    private float timer;
    [SerializeField] int leftXPos;
    [SerializeField] int rightXPos;
    [SerializeField] int sizeDecrease;
    [SerializeField] bool attacking;
    [SerializeField] bool flipped;
    [SerializeField] bool dead;

    Vector3 currentPosition;
    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        BounceRate();
        AttackScales();
        if (dead) 
        { 
            return;
        }
        if (!attacking)
        {
            MatchPlayerPos();
        }
        else
        {
            Slam();
        }
    }

    void MatchPlayerPos() 
    {

        currentPosition.y = player.position.y;

        transform.position = currentPosition;
    }

    void Slam() 
    {
        if (currentPosition.x > leftXPos && !flipped)
        {
            currentPosition.x -= speed * Time.deltaTime;
            transform.position = currentPosition;
            if (currentPosition.x <= leftXPos && !flipped) 
            { 
                flipped = true;
                attacking = false;
                currentAttackNumber++;
            }
        }
        else if (currentPosition.x < rightXPos && flipped)
        {
            currentPosition.x += speed * Time.deltaTime;
            transform.position = currentPosition;
            if (currentPosition.x >= rightXPos && flipped)
            {
                flipped = false;
                attacking = false;
                currentAttackNumber++;
            }
        }
    }

    void BounceRate() 
    {
        timer += Time.deltaTime;
        if(timer > timeBtwAttack) 
        { 
            attacking = true;
            timer = 0;
        }
    }

    void Shrink() 
    {
        if (currentAttackNumber == attackNumber)
        {
            attackNumber= attackNumber + increaseAmount;
            currentAttackNumber = 0;
            transform.localScale -= new Vector3(sizeDecrease, sizeDecrease, sizeDecrease);
            if (transform.localScale.x < 0.1f) // Example minimum size
            {
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                Death();
            }
        }
    }

    void AttackScales() 
    {
        switch (attackNumber) 
        {
            case 1:
                increaseAmount = 1;
                rightXPos = 15;
                leftXPos = -15;
                break;
            case 2: 
                increaseAmount = 2;
                rightXPos = 16;
                leftXPos = -16;
                break;
            case 3: 
                increaseAmount = 3;
                rightXPos = -17;
                leftXPos = -17;
                break;
            default:
                increaseAmount = 1;
                break;
        }
    }

    void Death() 
    {
        dead = true;
        spriteRenderer.color = Color.gray;
        Debug.Log("Death");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground") 
        { 
            Shrink();
            Debug.Log("Hit");
        }
        if(collision.tag == "Player") 
        {
            Debug.Log("restart");
        }
    }
}
