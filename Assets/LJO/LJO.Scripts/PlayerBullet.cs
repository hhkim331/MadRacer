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
    bool isFired = false; // �߰�: �Ѿ��� �߻�Ǿ����� Ȯ��

    bool isHit;
    Vector3 hitPosition;
    Quaternion hitRotation;
    public GameObject impactEffectPrefab; // �浹 ����Ʈ ������
    private float fireTime;  // �߻� �ð��� �����ϱ� ���� ���� �߰�

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;     // �ʱ⿡�� �߷��� �ۿ����� �ʵ��� ����
        rb.isKinematic = true;    // �ʱ⿡�� ������ �ൿ�� ���� �ʵ��� ����
        velocity = transform.forward * speed;
    }
    public void FireBullet() // �� �Լ��� �Ѿ��� �߻��� �� ȣ��
    {
        isFired = true;
        rb.isKinematic = false;
        rb.useGravity = false;
        velocity = transform.forward * speed;
        fireTime = Time.time; // �߻� �ð� ����

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
        if (isFired) // �߻�� ��쿡�� ����
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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (isHit == false)
    //    {
    //        isHit = true;
    //        hitRotation = transform.rotation;
    //        hitPosition = transform.position;
    //        rb.isKinematic = true;
    //        rb.useGravity = false;
    //        GetComponent<Collider>().enabled = false;

    //        // �Ѿ��� Enemy ������Ʈ�� �浹�� ���
    //        EnemyHP enemyHP = collision.gameObject.GetComponent<EnemyHP>();
    //        if (enemyHP != null) // ������ EnemyHP ��ũ��Ʈ�� �ִ� ���
    //        {
    //            float damageValue = 100f; // ���⼭�� ���÷� 100�� �������� ����
    //            enemyHP.Hit(damageValue, null); // null�� �Ѱ��־�����, �ʿ��� ��� ������ KHHKartRank ���� �Ѱ��ָ� �˴ϴ�.

    //            // �Ѿ��� Ȱ��ȭ ���¿��� ��Ȱ��ȭ ���·� ����
    //            this.gameObject.SetActive(false);
    //        }
    //    }

    //    if (isHit)
    //    {
    //        transform.rotation = hitRotation;
    //        transform.position = hitPosition;
    //    }
    //    else
    //    {
    //        rb.transform.forward = rb.velocity.normalized;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - fireTime < 0.1f) return;  // �� �κ� �߰�

        if (isHit == false)
        {
            Debug.Log("Bullet collided with: " + collision.gameObject.name);
            Debug.Log("Collision point: " + collision.contacts[0].point);
            isHit = true;
            hitRotation = transform.rotation;
            hitPosition = transform.position;

            GameObject impactEffect = Instantiate(impactEffectPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            Destroy(impactEffect, 1f);

            rb.isKinematic = true;
            rb.useGravity = false;
            GetComponent<Collider>().enabled = false;

            // �Ѿ��� Enemy ������Ʈ�� �浹�� ���
            EnemyHP enemyHP = collision.gameObject.GetComponent<EnemyHP>();
            if (enemyHP != null) // ������ EnemyHP ��ũ��Ʈ�� �ִ� ���
            {
                float damageValue = 100f; // ���⼭�� ���÷� 100�� �������� ����
                enemyHP.Hit(damageValue, null); // null�� �Ѱ��־�����, �ʿ��� ��� ������ KHHKartRank ���� �Ѱ��ָ� �˴ϴ�.

                // �Ѿ��� Ȱ��ȭ ���¿��� ��Ȱ��ȭ ���·� ����
                this.gameObject.SetActive(false);
            }
        }
        else if (isHit)
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
