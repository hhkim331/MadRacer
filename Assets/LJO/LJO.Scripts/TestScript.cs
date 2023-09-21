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
    private int bulletCount = 0;
    private float bowCreateTime;
    // Start is called before the first frame update
    private void Awake()
    {
        bowCreateTime = Time.time;
    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {

            UpdateAttack();

        }
        if (bulletCount >=3 )
        {
            DestroyBow();
        }
    }
    private void DestroyBow()
    {
        Destroy(gameObject);
    }



    private void UpdateAttack()
    {
       // var bullet = Instantiate(bowFactory);
        var bullet = Instantiate(bowFactory, FirePosition.position, FirePosition.rotation);

        bullet.transform.position = FirePosition.position;

        bulletCount++;
    }


    //public void ApplyItem(Item.ItemType itemType)
    //{
    //    switch (itemType)
    //    {
    //        case Item.ItemType.Bullet:

    //            print("총알충전");
    //            break;
    //        case Item.ItemType.Booster:
    //            print("부스터 충전");
    //            break;
    //        case Item.ItemType.attack:

    //            GameObject Instantiatebow = Instantiate(bow, inven.transform.position, Quaternion.identity, inven);

    //            print("석궁 충전");
    //            break;
    //        default:
    //            break;
    //    }
    //}

}