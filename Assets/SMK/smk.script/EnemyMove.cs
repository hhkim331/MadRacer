using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //움직이는곳.
    float speed = 1;
    float maxspeed = 10f;
    public Waypoint waypoint;//향하는 곳.
    Waypoint nextPoint;//다음 향할 곳
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, speed);
       
    }
}
