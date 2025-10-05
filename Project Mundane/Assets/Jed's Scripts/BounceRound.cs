using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BounceRound : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float returnSpeed;
    Vector2 Direction;
    public float radius;
    public float distance;
    public LayerMask whatIsSolid;
    public GameObject Boss;
    TriangleBoss bossCode;
    public bool returning;
    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("Boss");
        bossCode = FindFirstObjectByType<TriangleBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.right * distance, Color.red);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsSolid);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // Ignore self-detection if the script is on a collider that might overlap itself
            if (hitCollider.tag == "Bounce")
            {
                Debug.Log("Bounce Back");
                returning = true;
            }
            else if (hitCollider.tag == "Boss")
            {
                Debug.Log("Shrink");
                bossCode.Shrink();
                DestroyBullet();
            }
            else if(hitCollider.tag == "Ground") 
            { 
                DestroyBullet();
            }
        }
        if (!returning)
        {
            transform.Translate(Direction * speed * Time.deltaTime);
        }
        else 
        { 
            transform.position = Vector2.MoveTowards(transform.position, Boss.transform.position, returnSpeed * Time.deltaTime);
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
