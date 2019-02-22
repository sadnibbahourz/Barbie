using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerpos;
    public Vector3 offset;
    [Tooltip("How fast the camera moves")] [Range(0, 1)] public float smoothSpeed = 0.05f;
    Vector3 smoothedpos;
    void Start()
    {
        transform.position = playerpos.position+offset;
    }

    private void Update()
    {
        Constrains();
    }
    void FixedUpdate()
    {
        smoothedpos = Vector3.Lerp(transform.position, playerpos.position + offset, smoothSpeed);

        transform.position = smoothedpos;
    }

    void Constrains()
    {
        if(transform.position.x < 0)
        {
            transform.position = new Vector3(0,transform.position.y, transform.position.z);
        }

        if (transform.position.y < -0.018f)
        {
            transform.position = new Vector3(transform.position.x, -0.018f, transform.position.z);
        }
        
    }
}
