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
        //������Ʈ�� �ν��� ��ġ�� ���� ���ư�.

        if (ishit)
        {
            transform.rotation = hitRotation;
            transform.position = hitPosition;
        }
        else
        {
            rb.transform.forward = rb.velocity.normalized;
        }
        //�浹�� �ı�
    }
}
