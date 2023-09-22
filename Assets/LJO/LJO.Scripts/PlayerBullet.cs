using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    
    Rigidbody rb;
    public float speed = 10;
    Vector3 velocity;
    public Vector3 gravity = new Vector3 (0, -1f * 0.1f, 0);
    bool isFired = false; // 추가: 총알이 발사되었는지 확인

    bool isHit;
    Vector3 hitPosition;
    Quaternion hitRotation;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;     // 초기에는 중력이 작용하지 않도록 설정
        rb.isKinematic = true;    // 초기에는 물리적 행동을 받지 않도록 설정
        velocity = transform.forward * speed;
    }
    public void FireBullet() // 이 함수는 총알을 발사할 때 호출
    {
        isFired = true;
        rb.isKinematic = false;
        rb.useGravity = false;
        velocity = transform.forward * speed; 
    }
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent, false);
    }

    public void ReleaseParent()
    {
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFired) // 발사된 경우에만 실행
        {
            UpdateSpecialAttack();
        }


    }


    public GameObject bowFactory;
    public Transform FirePosition;

    private void UpdateSpecialAttack()
    {
        velocity += gravity * Time.deltaTime;
       
        
            transform.forward = velocity.normalized;
        
        transform.position += velocity * Time.deltaTime;

       
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
}
