//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyEye : MonoBehaviour
//{

//    public static EnemyEye Instance;
//    private void Awake()
//    {
//        Instance = this;
//    }

//    // 시야각 설정
//    public float viewRadius;
//    [Range(0, 360)]
//    public float viewAngle;

//    public LayerMask targetMask, obstacleMask;

//    // target 인식 후 저장 리스트
//    public List<Transform> visibleTargets = new List<Transform>();

//    void Start()
//    {
//        // 0.2초 간격으로 코루틴 호출
//        StartCoroutine(FindTargetsWithDelay(0.2f));
//    }

//    IEnumerator FindTargetsWithDelay(float delay)
//    {
//        while (true)
//        {
//            FindVisibleTargets();
//            yield return new WaitForSeconds(delay);
//        }
//    }

//    void FindVisibleTargets()
//    {
//        visibleTargets.Clear();
//        // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 collider를 모두 가져옴
//        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

//        for (int i = 0; i < targetsInViewRadius.Length; i++)
//        {
//            Transform target = targetsInViewRadius[i].transform;
//            Vector3 dirToTarget = (target.position - transform.position).normalized;

//            // 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
//            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
//            {
//                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

//                // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
//                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
//                {
//                    visibleTargets.Add(target);
//                }
//            }
//        }
//    }

//    // y축 오일러 각을 3차원 방향 벡터로 변환한다.
//    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
//    {
//        if (!angleIsGlobal)
//        {
//            angleDegrees += transform.eulerAngles.y;
//        }

//        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
//    }


//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEye : MonoBehaviour
{
    public static EnemyEye Instance;
    private void Awake()
    {
        Instance = this;
    }

    // 시야각 설정
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask, obstacleMask;

    // 오브젝트의 고유한 식별자
    public int objectId;

    // target 인식 후 저장 리스트
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        // 0.2초 간격으로 코루틴 호출
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            FindVisibleTargets();
            yield return new WaitForSeconds(delay);
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 collider를 모두 가져옴
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            // 고유한 식별자를 가진 오브젝트인 경우, 검출하지 않음
            EnemyEye enemyEye = target.GetComponentInParent<EnemyEye>();
            if (enemyEye != null && enemyEye.objectId == objectId)
            {
                continue;
            }

            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }
    // y축 오일러 각을 3차원 방향 벡터로 변환한다.
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }


}
