using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    
    public float smoothSpeed = 10f;
    public Vector3 offset;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = Player.instance.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt( Player.instance.transform);
    }
}
