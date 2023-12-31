using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    Rigidbody rb;
    public float speed = 30;
    Vector3 velocity;
    public Vector3 gravity = new Vector3(0, -1f * 0.1f, 0);
    bool isFired = false; // 추가: 총알이 발사되었는지 확인

    bool isHit;
    Vector3 hitPosition;
    Quaternion hitRotation;
    public GameObject impactEffectPrefab; // 충돌 이펙트 프리팹
    private float fireTime;  // 발사 시간을 저장하기 위한 변수 추가

    KHHKartRank kartRank;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;     // 초기에는 중력이 작용하지 않도록 설정
        rb.isKinematic = true;    // 초기에는 물리적 행동을 받지 않도록 설정
        velocity = transform.forward * speed;
    }

    public void Set(KHHKartRank kartRank)
    {
        this.kartRank = kartRank;
    }

    public void FireBullet(Vector3 fireVec) // 이 함수는 총알을 발사할 때 호출
    {
        isFired = true;
        rb.isKinematic = false;
        rb.useGravity = false;
        velocity = fireVec * speed;
        fireTime = Time.time; // 발사 시간 저장

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

    private void OnTriggerEnter(Collider other)
    {
        if (kartRank == null) return;
        Debug.Log("ffdfdf");
        if (other.CompareTag("Ground")) // Ground 태그를 확인하는 조건 추가
        {
            Debug.Log("Bullet collided with: " + other.gameObject.name);

            // 이펙트 생성
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            }

            // 총알 비활성화
            this.gameObject.SetActive(false);

            return;
        }
       
            //if (Time.time - fireTime < 1.5f) return;  // 이 부분 추가
            KHHKartRank hitkartRank = other.gameObject.GetComponentInParent<KHHKartRank>();
        if (hitkartRank == kartRank) return;

        if (isHit == false)
        {
            Debug.Log("Bullet collided with: " + other.gameObject.name);
            //Debug.Log("Collision point: " + collision.contacts[0].point);
            isHit = true;
            hitRotation = transform.rotation;
            hitPosition = transform.position;

            // GameObject impactEffect = Instantiate(impactEffectPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            // Destroy(impactEffect, 1f);

            if (impactEffectPrefab != null) // impactEffectPrefab가 null이 아닌지 확인
            {
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            }

            rb.isKinematic = true;
            rb.useGravity = false;
            GetComponent<Collider>().enabled = false;

            // 총알이 Enemy 오브젝트와 충돌한 경우
            KHHHealth enemyHP = other.gameObject.GetComponentInParent<KHHHealth>();
            if (enemyHP != null) // 적에게 EnemyHP 스크립트가 있는 경우
            {
                float damageValue = 100f; // 여기서는 예시로 100의 데미지를 가정
                enemyHP.Hit(damageValue, kartRank); // null을 넘겨주었지만, 필요한 경우 적절한 KHHKartRank 값을 넘겨주면 됩니다.

                // 총알을 활성화 상태에서 비활성화 상태로 변경
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
