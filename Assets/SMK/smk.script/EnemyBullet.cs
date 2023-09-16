using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed = 400f;

    private void Start()
    {
        transform.position += transform.forward * speed;
    }
}
