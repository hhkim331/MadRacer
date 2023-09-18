using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_move : MonoBehaviour
{
    public float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); // 좌, 우 이동을 위한 입력값 (A, D 또는 왼쪽, 오른쪽 화살표)
        float vertical = Input.GetAxis("Vertical");     // 앞, 뒤 이동을 위한 입력값 (W, S 또는 위, 아래 화살표)

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * speed * Time.deltaTime;

        transform.Translate(movement);
    }
}
