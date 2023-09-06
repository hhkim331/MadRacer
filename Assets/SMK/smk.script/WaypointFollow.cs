using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    //waypoint�� ���ϰ� ��.

    //����, waypoint�� ������, ������Ʈ�� �չ��⿡ ��ġ�� ���� waypoint�� ������.
    //nextpoint�� �ν��ؼ� �ش� ��ġ�� �̵��ϱ�. 

    Transform enemypoint;//enemypoint�� ���� �ڽ�
    public Waypoint waypoint;//���ϴ� ��.
    Waypoint nextPoint;//���� ���� ��
    void Start()
    {
        if (waypoint != null)
        {
            nextPoint = waypoint.nextPoint;
        }
    }

    void Update()
    {
        //(waypoint�� enemypoint ���� �Ÿ�)�� ������ waypoint�� �������� �������� �۰ų� ���� �� (= waypoint�� enemypoint���� �Ÿ��� ª����)
        if ((waypoint.transform.position - enemypoint.transform.position).sqrMagnitude <= waypoint.radius * waypoint.radius)
        {
            print("2");
            nextPoint = waypoint.nextPoint;
            if(waypoint != null)
            {
                nextPoint = waypoint.nextPoint;
            }
        }
        print("1");
        //waypoint�� �̵�
        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, 0.1f);
    }
}
