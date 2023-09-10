using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    Rigidbody rb;
    public float speed = 10f;
    bool ishit;

    Quaternion hitRotation;
    Vector3 hitPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    void Update()
    {
        //오브젝트를 인식한 위치를 향해 날아감.

        if (ishit)
        {
            transform.rotation = hitRotation;
            transform.position = hitPosition;
        }
        else
        {
            rb.transform.forward = rb.velocity.normalized;
        }
        //충돌시 파괴
    }

}
