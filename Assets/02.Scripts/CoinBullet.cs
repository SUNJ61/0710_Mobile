using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject Effect;

    private float Speed = 1000.0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * Speed);
        Destroy(gameObject, 1.5f);
    }
}
