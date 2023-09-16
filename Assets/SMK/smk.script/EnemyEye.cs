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

//    // �þ߰� ����
//    public float viewRadius;
//    [Range(0, 360)]
//    public float viewAngle;

//    public LayerMask targetMask, obstacleMask;

//    // target �ν� �� ���� ����Ʈ
//    public List<Transform> visibleTargets = new List<Transform>();

//    void Start()
//    {
//        // 0.2�� �������� �ڷ�ƾ ȣ��
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
//        // viewRadius�� ���������� �� �� ���� �� targetMask ���̾��� collider�� ��� ������
//        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

//        for (int i = 0; i < targetsInViewRadius.Length; i++)
//        {
//            Transform target = targetsInViewRadius[i].transform;
//            Vector3 dirToTarget = (target.position - transform.position).normalized;

//            // �÷��̾�� forward�� target�� �̷�� ���� ������ ���� �����
//            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
//            {
//                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

//                // Ÿ������ ���� ����ĳ��Ʈ�� obstacleMask�� �ɸ��� ������ visibleTargets�� Add
//                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
//                {
//                    visibleTargets.Add(target);
//                }
//            }
//        }
//    }

//    // y�� ���Ϸ� ���� 3���� ���� ���ͷ� ��ȯ�Ѵ�.
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

    // �þ߰� ����
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask, obstacleMask;

    // ������Ʈ�� ������ �ĺ���
    public int objectId;

    // target �ν� �� ���� ����Ʈ
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        // 0.2�� �������� �ڷ�ƾ ȣ��
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
        // viewRadius�� ���������� �� �� ���� �� targetMask ���̾��� collider�� ��� ������
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            // ������ �ĺ��ڸ� ���� ������Ʈ�� ���, �������� ����
            EnemyEye enemyEye = target.GetComponentInParent<EnemyEye>();
            if (enemyEye != null && enemyEye.objectId == objectId)
            {
                continue;
            }

            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // �÷��̾�� forward�� target�� �̷�� ���� ������ ���� �����
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // Ÿ������ ���� ����ĳ��Ʈ�� obstacleMask�� �ɸ��� ������ visibleTargets�� Add
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }
    // y�� ���Ϸ� ���� 3���� ���� ���ͷ� ��ȯ�Ѵ�.
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }


}
