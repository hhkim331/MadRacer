using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //�����̴°�.
    float speed = 1;
    float maxspeed = 10f;
    public Waypoint waypoint;//���ϴ� ��.
    Waypoint nextPoint;//���� ���� ��
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, speed);
       
    }
}
