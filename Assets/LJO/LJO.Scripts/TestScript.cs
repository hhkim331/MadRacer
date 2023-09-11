using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Item;

public class TestScript : MonoBehaviour
{
    public static TestScript Instance;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            default:
                break;
        }
    }
}
