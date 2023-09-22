using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaypointFollow : MonoBehaviour
{
    //waypoint로 향하게 함.
    //만약, waypoint를 지나면, 오브젝트의 앞방향에 위치한 다음 waypoint를 방향삼기.
    //nextpoint로 이동하기전에 돌린 rotation 이 일정 이상이면, 드리프트 하게 바꾸기.

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

    //부스터
    public LayerMask groundLr;
    public Vector3 groundBx = Vector3.zero;


    public KHHWaypoint waypoint;//향하는 곳.
    public float normalSpeed = 0.1f;//평소 속도
    public float acceleration = 2f;//가속
    public float breakForce = 0.01f;//감속
    public float speed;

    public float currentTime;
    Rigidbody rb;


    int waypointIndex = -1;

    void Start()
    {
        currentTime = 0;
        speed = normalSpeed;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    bool isRot;
    public Transform tr;
    void Update()
    {
        if (!KHHGameManager.instance.isStart) return;

        UpdateFollow();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isRot = true;
        }

        if (isRot)
        {
            //회전
            Vector3 dir = tr.position - transform.position;
            dir.y = 0;
            Vector3 dir2 = transform.forward;
            dir2.y = 0;
            dir = Vector3.Lerp(dir2, dir, Time.deltaTime);
            dir.y = transform.forward.y;
            transform.forward = dir;
        }

    }


    void UpdateFollow()
    {
        //이동
        //피격, 시작 후 시간 흐름으로 속도 증가.
        //if (!KHHGameManager.instance.isStart) return;

        transform.position += transform.forward * speed * Time.deltaTime;



        //회전
        Vector3 dir = waypoint.wayPosition - transform.position;
        dir.y = 0;
        Vector3 dir2 = transform.forward;
        dir2.y = 0;
        dir = Vector3.Lerp(dir2, dir, Time.deltaTime * 0.1f);
        dir.y = transform.forward.y;
        transform.forward = dir;
    }



    //충돌시 다음 waypoint로 변경
    private void OnTriggerEnter(Collider other)
    {
        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();

        currentTime += Time.deltaTime;
        if (hitWaypoint == null) return;
        //waypoint 
        if (waypointIndex == hitWaypoint.waypointIndex) return;
        waypointIndex = hitWaypoint.waypointIndex;
        waypoint = hitWaypoint.NextPoint();

        StartCoroutine(speedchange());
        speed = normalSpeed;
    }

    //시간 야매로 조정하기.
    IEnumerator speedchange()
    {
        speed = acceleration;
        yield return new WaitForSeconds(3);
    }

}
#region
//public class WaypointFollow : MonoBehaviour
//{
//    //waypoint로 향하게 함.
//    //만약, waypoint를 지나면, 오브젝트의 앞방향에 위치한 다음 waypoint를 방향삼기.
//    //nextpoint를 인식해서 해당 위치로 이동하기. 


//    #region drift 변수
//    public float maxdriftAngle;
//    public float mindriftAngle;
//    public float mindriftSpeed;
//    public float drifttime;
//    public float driftswingPos;
//    #endregion
//    //드리프트
//    //-자동차를 미끄러트려 컨트롤.
//    //차체 후미
//    //부스터

//    bool isGround = false;
//    public LayerMask groundLr;
//    public Vector3 groundBx = Vector3.zero;


//    public KHHWaypoint waypoint;//향하는 곳.
//    public float normalSpeed = 0.1f;//평소 속도
//    public float acceleration = 2f;//가속
//    public float breakForce = 0.01f;//감속
//    public float speed;
//    EnemyEye enemyEye;

//    Rigidbody rb;


//    int waypointIndex = -1;
//    void Start()
//    {
//        speed = normalSpeed;
//        rb = GetComponent<Rigidbody>();
//        rb.constraints = RigidbodyConstraints.FreezeRotationX;
//        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
//    }

//    void Update()
//    {
//        UpdateFollow();
//        //Vector3 waypointenemy = gameObject.transform.position- waypoint.transform.position;

//        //if (Isground())
//        //{
//        //    UpdateFollow();
//        //}
//        //이동방향을 enemy의 앞방향(z방향)으로 설정
//        //transform.forward = waypoint.transform.forward;
//        //(waypoint와 point 간의 거리)의 제곱이 waypoint의 반지름의 제곱보다 작거나 같을 때 (= waypoint랑 point간의 거리가 짧을때)
//        //충돌시 후진
//        //waypoint 거의 다오면, 속도 늦추기
//    }


//    void UpdateFollow()
//    {
//        //속도 변화 가까워지면 감속 / 멀면 가속
//        //멀면 가속
//        ////Vector3 dir= transform.position - hitWaypoint.GetComponentInParent<Vector3>();
//        //if (Vector3.Distance(hitWaypoint.transform.position, transform.position) >=0.5f)
//        //{
//        //    speed = acceleration;
//        //}
//        ////가까우면 감속
//        //else
//        //{
//        //    speed = breakForce;
//        //}
//        //이동
//        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, normalSpeed); //이동


//        ////회전
//        Vector3 dir = waypoint.transform.position - transform.position;
//        dir.y = 0;
//        Quaternion way = Quaternion.LookRotation(dir);
//        transform.rotation = way;
//        //Quaternion rot = Quaternion.Lerp(rb.rotation, dir, speed*Time.deltaTime);
//        //rb.rotation = Quaternion.Lerp(rb.rotation, way, speed * Time.deltaTime);

//    }
//    //IEnumerator TowardPlayer()
//    //{
//    //    // 3초 정도 시행하기

//    //    yield return new WaitForSeconds(0.3f);
//    //}Z

//    //ground 판정
//    bool Isground()
//    {
//        isGround = Physics.BoxCast(transform.position, groundBx * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f, groundLr);
//        return isGround;
//    }
//    //다음 waypoint 로 교체
//    private void OnTriggerEnter(Collider other)
//    {
//        //waypoint를 n개중에 하나로 바꾸기.
//        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();
//        if (hitWaypoint == null) return;
//        if (waypointIndex == hitWaypoint.waypointIndex) return;
//        waypointIndex = hitWaypoint.waypointIndex;
//        waypoint = hitWaypoint.NextPoint();

//        StartCoroutine(speedchange());
//        speed = normalSpeed;
//    }

//    //시간 야매로 조정하기.
//    IEnumerator speedchange()
//    {
//        speed = acceleration;
//        yield return new WaitForSeconds(3);
//    }

//}
#endregion