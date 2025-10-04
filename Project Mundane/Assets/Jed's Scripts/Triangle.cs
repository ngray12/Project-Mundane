using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpBoost;
    [SerializeField] Transform triangle;
    public bool isGrounded() 
    {
        float checkRadius = .4f;
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        return Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);
    }
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        Debug.Log(isGrounded());
    }

    // Update is called once per frame
    void Update()
    {
        float newXPos = transform.position.x + speed * Time.deltaTime;

        transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);
        Jump();
    }
    void Jump()
    {
        if (isGrounded())
        {
            Vector3 Rotation = triangle.rotation.eulerAngles;
            Rotation.z = Mathf.Round(Rotation.z / 120) * 120;
            triangle.rotation = Quaternion.Euler(Rotation);

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                rb.AddForce(Vector2.right * jumpBoost, ForceMode2D.Impulse);
            }
        }
        else
        {
            triangle.Rotate(Vector3.back * 904.8304372f * Time.deltaTime);
        }
    }
}
