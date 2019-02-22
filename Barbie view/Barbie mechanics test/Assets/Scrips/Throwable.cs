using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public float speed = 10;
    public Rigidbody2D rb;
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
}

