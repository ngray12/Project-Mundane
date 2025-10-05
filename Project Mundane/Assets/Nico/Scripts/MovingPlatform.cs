using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float speed = 5f; 

    void Update()
    {
        
        transform.position += Vector3.left * speed * Time.deltaTime;


        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Death"))
            {
                Debug.Log("Hit KillZone");
                Destroy(gameObject);
            }
        }
    }

    public void SetSpeed(float s) => speed = s;
}
