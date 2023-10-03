using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float rotationSpeed = 20f; // 초당 20도 회전
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("dsd");
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
