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
    public GameObject bow;
    public Transform inven;

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
                
                print("총알충전");
                break;
            case ItmeType.Booster:
                print("부스터 충전");
                break;
            case ItmeType.attack:

                GameObject Instantiatebow = Instantiate(bow,inven.transform.position,Quaternion.identity,inven);

                print("석궁 충전");
                break;
            default:
                break;
        }
    }

}
