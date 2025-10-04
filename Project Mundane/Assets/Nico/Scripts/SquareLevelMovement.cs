using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareLevelMovement : MonoBehaviour
{
    public float scrollSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
    }
}
