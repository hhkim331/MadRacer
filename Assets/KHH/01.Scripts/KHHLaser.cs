using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHLaser : MonoBehaviour
{
    private LineRenderer laser;        // 레이저
    private RaycastHit hitObj; // 충돌된 객체
    private GameObject curObj;   // 가장 최근에 충돌한 객체를 저장하기 위한 객체
    private Vector3 hitPoint;              // 충돌 지점
    public Vector3 HitPoint { get { return hitPoint; } }
    private Vector3 hitNormal;              // 충돌 지점
    public Vector3 HitNormal { get { return hitNormal; } }
    KHHTarget.HitType hitType = KHHTarget.HitType.None;
    public KHHTarget.HitType HitObjType { get { return hitType; } }
    public float Distance { get { return Vector3.Distance(transform.position, hitPoint); } }

    public LayerMask hitLayer; // 레이저 포인터가 충돌할 레이어
    public float raycastDistance = 1000f; // 레이저 포인터 감지 거리

    private void Start()
    {
        laser = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        laser.SetPosition(0, transform.position);

        // 충돌 감지 시
        if (Physics.Raycast(transform.position, transform.forward, out hitObj, raycastDistance, hitLayer))
        {
            laser.SetPosition(1, hitObj.point);
            hitPoint = hitObj.point;
            hitNormal = hitObj.normal;

            Debug.Log(hitObj.transform.name);

            KHHTarget target = hitObj.transform.GetComponent<KHHTarget>();
            if (target != null) hitType = target.hitType;
        }
        else
        {
            // 레이저에 감지된 것이 없기 때문에 레이저 초기 설정 길이만큼 길게 만든다.
            laser.SetPosition(1, transform.position + (transform.forward * raycastDistance));
            hitPoint = transform.position + (transform.forward * raycastDistance);
            hitType = KHHTarget.HitType.None;
        }
    }
}
