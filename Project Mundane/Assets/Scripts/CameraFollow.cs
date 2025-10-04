using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(1f, 0f, -10f);

    void LateUpdate()
    {
        //transform.position = player.position + offset;
    }
}
