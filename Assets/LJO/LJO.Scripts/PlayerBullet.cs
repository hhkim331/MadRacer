using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10;
    Vector3 velocity;
    public Vector3 gravity = new Vector3 (0, -9.81f * 0.1f, 0);
    // Start is called before the first frame update
    void Start()
    {
        rb =  GetComponent<Rigidbody>();
        rb.useGravity = false;
        velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpecialAttack();
    }

    public GameObject bowFactory;
    public Transform FirePosition;

    private void UpdateSpecialAttack()
    {
        velocity += gravity * Time.deltaTime;
        if (Input.GetButtonDown("Fire2"))
        {
            transform.forward = velocity.normalized;
        }
        transform.position += velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
