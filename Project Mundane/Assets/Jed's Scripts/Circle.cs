using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float thrust;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) 
        {
            rb.AddForce(Vector2.up * thrust* Time.deltaTime, ForceMode2D.Impulse);
        }
        else 
        {
            rb.AddForce(Vector2.up * -thrust * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}
