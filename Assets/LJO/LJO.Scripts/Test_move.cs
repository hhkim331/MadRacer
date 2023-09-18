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
        float horizontal = Input.GetAxis("Horizontal"); // ��, �� �̵��� ���� �Է°� (A, D �Ǵ� ����, ������ ȭ��ǥ)
        float vertical = Input.GetAxis("Vertical");     // ��, �� �̵��� ���� �Է°� (W, S �Ǵ� ��, �Ʒ� ȭ��ǥ)

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * speed * Time.deltaTime;

        transform.Translate(movement);
    }
}
