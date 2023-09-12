using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    //waypoint로 향하게 함.
    //만약, waypoint를 지나면, 오브젝트의 앞방향에 위치한 다음 waypoint를 방향삼기.
    //nextpoint를 인식해서 해당 위치로 이동하기. 


    #region drift 변수
    public float maxdriftAngle;
    public float mindriftAngle;
    public float mindriftSpeed;
    public float drifttime;
    public float driftswingPos;
    #endregion
    //드리프트
    //-자동차를 미끄러트려 컨트롤.
    //차체 후미
    //부스터

    bool isGround = false;
    public LayerMask groundLr;
    public Vector3 groundBx = Vector3.zero;


    public Waypoint waypoint;//향하는 곳.
    Waypoint nextPoint;//다음 향할 곳
    public float normalSpeed = 0.1f;//평소 속도
    public float acceleration = 2f;//가속
    public float breakForce = 0.01f;//감속
    void Start()
    {

    }

    void Update()
    {
        if (Isground())
        {
            UpdateFollow();
        }
        //이동방향을 enemy의 앞방향(z방향)으로 설정
        //transform.forward = waypoint.transform.forward;
        //(waypoint와 point 간의 거리)의 제곱이 waypoint의 반지름의 제곱보다 작거나 같을 때 (= waypoint랑 point간의 거리가 짧을때)
        //충돌시 후진
        //waypoint 거의 다오면, 속도 늦추기
        //방향의 범위를 정해서, 범위 외로 움직이면, 이펙트(파이어, 연기, 스키드마크) / 
    }

    public enum Enemystate { }


    void UpdateFollow()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, normalSpeed); //이동
        if ((waypoint.transform.position - transform.position).sqrMagnitude <= waypoint.radius * waypoint.radius)
        {
            //waypoint 변경
            waypoint = waypoint.nextPoint;
            if (waypoint != null)
            {
                nextPoint = waypoint.nextPoint;
            }
        }
    }
    void reverse()
    {

    }

    //ground 판정
    bool Isground()
    {
        isGround = Physics.BoxCast(transform.position, groundBx * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f, groundLr);
        return isGround;
    }
}
