using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalRound : MonoBehaviour
{
    [SerializeField] float speed;
    Vector2 Direction;
    public float radius;
    public float distance;
    public LayerMask whatIsSolid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsSolid);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // Ignore self-detection if the script is on a collider that might overlap itself
            if (hitCollider.tag == "Player")
            {
                Debug.Log("Restart");
            }
            DestroyBullet();
        }

       transform.Translate(Direction * speed * Time.deltaTime);
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
