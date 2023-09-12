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


    bool isHit;
    Vector3 hitPosition;
    Quaternion hitRotation;
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
       
        
            transform.forward = velocity.normalized;
        
        transform.position += velocity * Time.deltaTime;

        if (isHit)
        {
            transform.rotation = hitRotation;
            transform.position = hitPosition;
        }
        else
        {
            rb.transform.forward = rb.velocity.normalized;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHit == false)
        {
            isHit = true;
            hitRotation = transform.rotation;
            hitPosition = transform.position;
            rb.isKinematic = true;
            rb.useGravity = false;
            GetComponent<Collider>().enabled = false;
            
        }
    }
}
