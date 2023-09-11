using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHLaser : MonoBehaviour
{
    private LineRenderer laser;        // ������
    private RaycastHit Collided_object; // �浹�� ��ü
    private GameObject currentObject;   // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü
    private Vector3 hitPoint;              // �浹 ����
    public Vector3 HitPoint { get { return hitPoint; } }

    public float raycastDistance = 100f; // ������ ������ ���� �Ÿ�

    private void Start()
    {
        laser = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        laser.SetPosition(0, transform.position);

        // �浹 ���� ��
        if (Physics.Raycast(transform.position, transform.forward, out Collided_object, raycastDistance))
        {
            laser.SetPosition(1, Collided_object.point);
            hitPoint = Collided_object.point;
        }
        else
        {
            // �������� ������ ���� ���� ������ ������ �ʱ� ���� ���̸�ŭ ��� �����.
            laser.SetPosition(1, transform.position + (transform.forward * raycastDistance));
            hitPoint = transform.position + (transform.forward * raycastDistance);
        }
    }
}
