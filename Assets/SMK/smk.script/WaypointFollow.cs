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


    public KHHWaypoint waypoint;//향하는 곳.
    KHHWaypoint nextPoint;//다음 향할 곳
    public float normalSpeed = 0.1f;//평소 속도
    public float acceleration = 2f;//가속
    public float breakForce = 0.01f;//감속
    public float speed;
    EnemyEye enemyEye;

    int waypointIndex = -1;
    void Start()
    {
        speed = normalSpeed;
    }

    void Update()
    {
        UpdateFollow();
        Vector3 waypointenemy = gameObject.transform.position- waypoint.transform.position;

        //if (Isground())
        //{
        //    UpdateFollow();
        //}
        //이동방향을 enemy의 앞방향(z방향)으로 설정
        //transform.forward = waypoint.transform.forward;
        //(waypoint와 point 간의 거리)의 제곱이 waypoint의 반지름의 제곱보다 작거나 같을 때 (= waypoint랑 point간의 거리가 짧을때)
        //충돌시 후진
        //waypoint 거의 다오면, 속도 늦추기
        //방향의 범위를 정해서, 범위 외로 움직이면, 이펙트(파이어, 연기, 스키드마크) / 
    }


    void UpdateFollow()
    {
        //속도 변화 가까워지면 감속 / 멀면 가속
        //멀면 가속
        //if ()
        //{
        //    speed = acceleration;
        //}
        ////가까우면 감속
        //else
        //{
        //    speed = breakForce;
        //}
        //이동
        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, normalSpeed); //이동
        ////if ((waypoint.transform.position - transform.position).sqrMagnitude <= waypoint.radius * waypoint.radius)
        //{

        //    //waypoint 변경
        //    waypoint = waypoint.nextPoint;
        //    if (waypoint != null)
        //    {
        //        nextPoint = waypoint.nextPoint;
        //        //nextpoint를 리스트로 작성해서 랜덤하게 가져오는 방식으로 움직이는걸 바꾸기
        //    }
        //}
        // enemyEye.visibleTargets[0]가 있을때, 약 몇초 동안 enemyEye.visibleTargets[0]로 향하게 하기
        //if (enemyEye.visibleTargets != null)
        //{
        //    StartCoroutine(TowardPlayer());
        //}

    }
    IEnumerator TowardPlayer()
    {
        // 3초 정도 시행하기
        transform.position = Vector3.MoveTowards(gameObject.transform.position, enemyEye.visibleTargets[0].transform.position, normalSpeed);
        yield return new WaitForSeconds(3);
    }
    
    //ground 판정
    bool Isground()
    {
        isGround = Physics.BoxCast(transform.position, groundBx * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f, groundLr);
        return isGround;
    }
    private void OnTriggerEnter(Collider other)
    {
        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();
        if(hitWaypoint == null) return;
        if (waypointIndex == hitWaypoint.waypointIndex) return;
        waypointIndex = hitWaypoint.waypointIndex;
        waypoint = hitWaypoint.NextPoint();

        if (waypoint != null)
        {
            //nextPoint = waypoint.nextPoint;
            //nextpoint를 리스트로 작성해서 랜덤하게 가져오는 방식으로 움직이는걸 바꾸기
        }
    }
}
