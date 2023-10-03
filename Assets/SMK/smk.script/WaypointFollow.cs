using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class WaypointFollow : MonoBehaviour
{
    //waypoint�� ���ϰ� ��.
    //����, waypoint�� ������, ������Ʈ�� �չ��⿡ ��ġ�� ���� waypoint�� ������.
    //nextpoint�� �̵��ϱ����� ���� rotation �� ���� �̻��̸�, �帮��Ʈ �ϰ� �ٲٱ�.

    //nextpoint�� �ν��ؼ� �ش� ��ġ�� �̵��ϱ�. 


    #region drift ����
    public float mindriftSpeed;
    public float drifttime;
    public float driftswingPos;
    public GameObject driftLEffect;
    public GameObject driftREffect;
    public float waypointAngleValue;
    public float currentAngleValue;

    //�帮��Ʈ
    //-�ڵ����� �̲���Ʈ�� ��Ʈ��.

    //�ν���
    public LayerMask groundLr;
    public Vector3 groundBx = Vector3.zero;


    public KHHWaypoint waypoint;//���ϴ� ��.
    public float normalSpeed = 36;//��� �ӵ�
    public float acceleration = 55f;//����
    public float breakForce = 0.01f;//����
    public float max;
    public float speed;

    public float currentTime;
    Rigidbody rb;


    int waypointIndex = -1;

    #endregion
    void Start()
    {
        //����,waypoint rotation�� ����.
        currentAngleValue = gameObject.transform.rotation.eulerAngles.y;
        waypointAngleValue = waypoint.transform.rotation.eulerAngles.y;

        currentTime = 0;
        speed = 0;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;

        driftLEffect.SetActive(false);
        driftREffect.SetActive(false);
        max = normalSpeed;
    }

    bool isRot;
    public Transform tr;
    void Update()
    {
        if (!KHHGameManager.instance.isStart) return;
        UpdateFollow();

    }


    //�̵�
    void UpdateFollow()
    {
        
        //���� �� �ð� �帧���� �ӵ� ����.
        speed += 5 * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, max);
        transform.position += transform.forward * speed * Time.deltaTime;
        //���� ����.
        EnemySound.Instance.Move();


        //ȸ��
        Vector3 dir = waypoint.wayPosition - transform.position;
       // dir.y = 0;
        Vector3 dir2 = transform.forward;
        //dir2.y = 0;
        dir = Vector3.MoveTowards(dir2, dir, Time.deltaTime * 4f);
        //dir.y = transform.forward.y;
        transform.forward = dir;

    }



    //�浹�� ���� waypoint�� ����
    public void OnTriggerEnter(Collider other)
    {

        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();

        currentTime += Time.deltaTime;
        if (hitWaypoint == null) return;
        //waypoint 
        if (waypointIndex == hitWaypoint.waypointIndex) return;
        waypointIndex = hitWaypoint.waypointIndex;
        waypoint = hitWaypoint.NextPoint();

        //���� nextpoint�� angle �� �����ؼ�,
        //waypoint�� �浹������ + ������ angle���� +30�� �ϰ�� ����������, -30���ϰ�� �������� ����Ʈ
        //�� ȸ���ϰ� �ϱ�.
        if (waypointAngleValue == currentAngleValue + 30)
        {
            Debug.Log("dL");
            driftLEffect.SetActive(true);
            driftLEffect.SetActive(false);
        }
        else if (waypointAngleValue == currentAngleValue - 30)
        {
            Debug.Log("dR");
            driftREffect.SetActive(true);
            driftREffect.SetActive(false);
        }

        //�浹������ rotation�� ����.
        currentAngleValue = gameObject.transform.rotation.eulerAngles.y;
        waypointAngleValue = waypoint.transform.rotation.eulerAngles.y;


    }


    void drift()
    {

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