using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaypointFollow : MonoBehaviour
{
    //waypoint�� ���ϰ� ��.
    //����, waypoint�� ������, ������Ʈ�� �չ��⿡ ��ġ�� ���� waypoint�� ������.
    //nextpoint�� �̵��ϱ����� ���� rotation �� ���� �̻��̸�, �帮��Ʈ �ϰ� �ٲٱ�.

    //nextpoint�� �ν��ؼ� �ش� ��ġ�� �̵��ϱ�. 


    #region drift ����
    public float maxdriftAngle;
    public float mindriftAngle;
    public float mindriftSpeed;
    public float drifttime;
    public float driftswingPos;
    #endregion
    //�帮��Ʈ
    //-�ڵ����� �̲���Ʈ�� ��Ʈ��.

    //�ν���
    public LayerMask groundLr;
    public Vector3 groundBx = Vector3.zero;


    public KHHWaypoint waypoint;//���ϴ� ��.
    public float normalSpeed = 0.1f;//��� �ӵ�
    public float acceleration = 2f;//����
    public float breakForce = 0.01f;//����
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
        //if (!KHHGameManager.instance.isStart) return;

        UpdateFollow();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isRot = true;
        }

        if (isRot)
        {
            //ȸ��
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
        //�̵�
        //�ǰ�, ���� �� �ð� �帧���� �ӵ� ����.
        //if (!KHHGameManager.instance.isStart) return;

        transform.position += transform.forward * speed * Time.deltaTime;



        //ȸ��
        Vector3 dir = waypoint.wayPosition - transform.position;
        dir.y = 0;
        Vector3 dir2 = transform.forward;
        dir2.y = 0;
        dir = Vector3.Lerp(dir2, dir, Time.deltaTime * 0.1f);
        dir.y = transform.forward.y;
        transform.forward = dir;
    }



    //�浹�� ���� waypoint�� ����
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

    //�ð� �߸ŷ� �����ϱ�.
    IEnumerator speedchange()
    {
        speed = acceleration;
        yield return new WaitForSeconds(3);
    }

}
#region
//public class WaypointFollow : MonoBehaviour
//{
//    //waypoint�� ���ϰ� ��.
//    //����, waypoint�� ������, ������Ʈ�� �չ��⿡ ��ġ�� ���� waypoint�� ������.
//    //nextpoint�� �ν��ؼ� �ش� ��ġ�� �̵��ϱ�. 


//    #region drift ����
//    public float maxdriftAngle;
//    public float mindriftAngle;
//    public float mindriftSpeed;
//    public float drifttime;
//    public float driftswingPos;
//    #endregion
//    //�帮��Ʈ
//    //-�ڵ����� �̲���Ʈ�� ��Ʈ��.
//    //��ü �Ĺ�
//    //�ν���

//    bool isGround = false;
//    public LayerMask groundLr;
//    public Vector3 groundBx = Vector3.zero;


//    public KHHWaypoint waypoint;//���ϴ� ��.
//    public float normalSpeed = 0.1f;//��� �ӵ�
//    public float acceleration = 2f;//����
//    public float breakForce = 0.01f;//����
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
//        //�̵������� enemy�� �չ���(z����)���� ����
//        //transform.forward = waypoint.transform.forward;
//        //(waypoint�� point ���� �Ÿ�)�� ������ waypoint�� �������� �������� �۰ų� ���� �� (= waypoint�� point���� �Ÿ��� ª����)
//        //�浹�� ����
//        //waypoint ���� �ٿ���, �ӵ� ���߱�
//    }


//    void UpdateFollow()
//    {
//        //�ӵ� ��ȭ ��������� ���� / �ָ� ����
//        //�ָ� ����
//        ////Vector3 dir= transform.position - hitWaypoint.GetComponentInParent<Vector3>();
//        //if (Vector3.Distance(hitWaypoint.transform.position, transform.position) >=0.5f)
//        //{
//        //    speed = acceleration;
//        //}
//        ////������ ����
//        //else
//        //{
//        //    speed = breakForce;
//        //}
//        //�̵�
//        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, normalSpeed); //�̵�


//        ////ȸ��
//        Vector3 dir = waypoint.transform.position - transform.position;
//        dir.y = 0;
//        Quaternion way = Quaternion.LookRotation(dir);
//        transform.rotation = way;
//        //Quaternion rot = Quaternion.Lerp(rb.rotation, dir, speed*Time.deltaTime);
//        //rb.rotation = Quaternion.Lerp(rb.rotation, way, speed * Time.deltaTime);

//    }
//    //IEnumerator TowardPlayer()
//    //{
//    //    // 3�� ���� �����ϱ�

//    //    yield return new WaitForSeconds(0.3f);
//    //}Z

//    //ground ����
//    bool Isground()
//    {
//        isGround = Physics.BoxCast(transform.position, groundBx * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f, groundLr);
//        return isGround;
//    }
//    //���� waypoint �� ��ü
//    private void OnTriggerEnter(Collider other)
//    {
//        //waypoint�� n���߿� �ϳ��� �ٲٱ�.
//        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();
//        if (hitWaypoint == null) return;
//        if (waypointIndex == hitWaypoint.waypointIndex) return;
//        waypointIndex = hitWaypoint.waypointIndex;
//        waypoint = hitWaypoint.NextPoint();

//        StartCoroutine(speedchange());
//        speed = normalSpeed;
//    }

//    //�ð� �߸ŷ� �����ϱ�.
//    IEnumerator speedchange()
//    {
//        speed = acceleration;
//        yield return new WaitForSeconds(3);
//    }

//}
#endregion