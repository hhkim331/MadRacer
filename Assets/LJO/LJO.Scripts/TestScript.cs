using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Item;

public class TestScript : MonoBehaviour
{
    public static TestScript Instance;

    public GameObject bowFactory;

    public Transform FirePosition;

    Rigidbody rb;
    public float speed = 10;
    Vector3 velocity;
    public Vector3 gravity = new Vector3(0, -9.81f * 0.1f, 0);
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {

            UpdateAttack();

        }
    }

    private void UpdateAttack()
    {
        var bullet = Instantiate(bowFactory);

        velocity += gravity * Time.deltaTime;

        bullet.transform.position = FirePosition.position;
        transform.forward = velocity.normalized;

        transform.position += velocity * Time.deltaTime;
    }

    public void ApplyItem(ItmeType itemType)
    {
        switch (itemType)
        {
            case ItmeType.Bullet:
                print("�Ѿ�����");
                break;
            case ItmeType.Booster:
                print("�ν��� ����");
                break;
            default:
                break;
        }
    }

}
