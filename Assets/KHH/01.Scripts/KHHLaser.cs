using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHLaser : MonoBehaviour
{
    private LineRenderer laser;        // ������
    private RaycastHit hit; // �浹�� ��ü
    private GameObject curObj;   // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü
    private Vector3 hitPoint;              // �浹 ����
    public Vector3 HitPoint { get { return hitPoint; } }
    private Vector3 hitNormal;              // �浹 ����
    public Vector3 HitNormal { get { return hitNormal; } }
    public GameObject hitObj;
    public float Distance { get { return Vector3.Distance(transform.position, hitPoint); } }

    public LayerMask hitLayer; // ������ �����Ͱ� �浹�� ���̾�
    public float raycastDistance = 1000f; // ������ ������ ���� �Ÿ�

    private void Start()
    {
        laser = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        laser.SetPosition(0, transform.position);

        // �浹 ���� ��
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, hitLayer))
        {
            laser.SetPosition(1, hit.point);
            hitPoint = hit.point;
            hitNormal = hit.normal;
            hitObj = hit.collider.gameObject;
        }
        else
        {
            // �������� ������ ���� ���� ������ ������ �ʱ� ���� ���̸�ŭ ��� �����.
            laser.SetPosition(1, transform.position + (transform.forward * raycastDistance));
            hitPoint = transform.position + (transform.forward * raycastDistance);
        }
    }
}
