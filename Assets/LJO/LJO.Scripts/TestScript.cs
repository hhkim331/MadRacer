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
