using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, 0.1f);
    }
}
