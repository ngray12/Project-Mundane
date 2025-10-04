using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UIElements;

public class StarPlayerMovement : MonoBehaviour
{

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;
    public float minAngle = -90f;
    public float maxAngle = 90f;
    //public float rotationStep = 72f;

    public float currentRotation = 0f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public float shootInterval = 1f;
    public float projectileSpeed = 10f;

    private float shootTimer = 0f;


    private void Start()
    {
        currentRotation = (minAngle + maxAngle) / 2f;
        transform.localRotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    // Update is called once per frame
    void Update()
    {
        //  -------- Free Rotation --------------------
        float input = Input.GetAxis("Vertical");
        float rotationAmmount = transform.eulerAngles.z;

        if (rotationAmmount > 180f) rotationAmmount -= 360f;

        rotationAmmount += input * rotationSpeed * Time.deltaTime;

        rotationAmmount = Mathf.Clamp(rotationAmmount, minAngle, maxAngle);

        transform.rotation=Quaternion.Euler(0f,0f, rotationAmmount);
        
        

        /* --------- Steped Rotation --------------
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Rotate(rotationStep);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Rotate(-rotationStep);
        }
        */

        //Shoot
        shootTimer += Time.deltaTime;
        if (shootTimer > shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Rotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0f,0f,transform.eulerAngles.z + angle);
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * projectileSpeed;
        }
    }

    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float radius = 1f; // adjust for visual size
        Vector3 center = transform.position;

        // Draw min angle line
        Vector3 minDir = Quaternion.Euler(0, 0, minAngle) * Vector3.up * radius;
        Gizmos.DrawLine(center, center + minDir);

        // Draw max angle line
        Vector3 maxDir = Quaternion.Euler(0, 0, maxAngle) * Vector3.up * radius;
        Gizmos.DrawLine(center, center + maxDir);

        // Draw arc for visualization
        int steps = 20;
        for (int i = 0; i < steps; i++)
        {
            float t1 = Mathf.Lerp(minAngle, maxAngle, (float)i / steps);
            float t2 = Mathf.Lerp(minAngle, maxAngle, (float)(i + 1) / steps);

            Vector3 p1 = center + Quaternion.Euler(0, 0, t1) * Vector3.up * radius;
            Vector3 p2 = center + Quaternion.Euler(0, 0, t2) * Vector3.up * radius;

            Gizmos.DrawLine(p1, p2);
        }
    }


}
