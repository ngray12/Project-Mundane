using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CamMover : MonoBehaviour
{
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCam();
    }

    void MoveCam() 
    { 
        float newXPos = transform.position.x + speed * Time.deltaTime;

        transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);

    }
}
