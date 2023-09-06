using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    //waypoint로 향하게 함.

    //만약, waypoint를 지나면, 오브젝트의 앞방향에 위치한 다음 waypoint를 방향삼기.
    //nextpoint를 인식해서 해당 위치로 이동하기. 

    Transform enemypoint;//enemypoint는 현재 자신
    public Waypoint waypoint;//향하는 곳.
    Waypoint nextPoint;//다음 향할 곳
    void Start()
    {
        if (waypoint != null)
        {
            nextPoint = waypoint.nextPoint;
        }
    }

    void Update()
    {
        //(waypoint와 enemypoint 간의 거리)의 제곱이 waypoint의 반지름의 제곱보다 작거나 같을 때 (= waypoint랑 enemypoint간의 거리가 짧을때)
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
        //waypoint로 이동
        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, 0.1f);
    }
}
