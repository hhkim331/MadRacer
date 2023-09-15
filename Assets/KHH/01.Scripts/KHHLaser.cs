using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHLaser : MonoBehaviour
{
    private LineRenderer laser;        // ������
    private RaycastHit hitObj; // �浹�� ��ü
    private GameObject curObj;   // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü
    private Vector3 hitPoint;              // �浹 ����
    public Vector3 HitPoint { get { return hitPoint; } }
    private Vector3 hitNormal;              // �浹 ����
    public Vector3 HitNormal { get { return hitNormal; } }
    KHHTarget.HitType hitType = KHHTarget.HitType.None;
    public KHHTarget.HitType HitObjType { get { return hitType; } }
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
        if (Physics.Raycast(transform.position, transform.forward, out hitObj, raycastDistance, hitLayer))
        {
            laser.SetPosition(1, hitObj.point);
            hitPoint = hitObj.point;
            hitNormal = hitObj.normal;

            KHHTarget target = hitObj.transform.GetComponent<KHHTarget>();
            if (target != null) hitType = target.hitType;
        }
        else
        {
            // �������� ������ ���� ���� ������ ������ �ʱ� ���� ���̸�ŭ ��� �����.
            laser.SetPosition(1, transform.position + (transform.forward * raycastDistance));
            hitPoint = transform.position + (transform.forward * raycastDistance);
            hitType = KHHTarget.HitType.None;
        }
    }
}
