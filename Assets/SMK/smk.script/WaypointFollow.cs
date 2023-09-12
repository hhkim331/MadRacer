using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class WaypointFollow : MonoBehaviour
{
    //waypoint�� ���ϰ� ��.
    //����, waypoint�� ������, ������Ʈ�� �չ��⿡ ��ġ�� ���� waypoint�� ������.
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
    //��ü �Ĺ�
    //�ν���

    bool isGround = false;
    public LayerMask groundLr;
    public Vector3 groundBx = Vector3.zero;


    public KHHWaypoint waypoint;//���ϴ� ��.
    KHHWaypoint nextPoint;//���� ���� ��
    public float normalSpeed = 0.1f;//��� �ӵ�
    public float acceleration = 2f;//����
    public float breakForce = 0.01f;//����
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
        //Vector3 waypointenemy = gameObject.transform.position- waypoint.transform.position;

        //if (Isground())
        //{
        //    UpdateFollow();
        //}
        //�̵������� enemy�� �չ���(z����)���� ����
        //transform.forward = waypoint.transform.forward;
        //(waypoint�� point ���� �Ÿ�)�� ������ waypoint�� �������� �������� �۰ų� ���� �� (= waypoint�� point���� �Ÿ��� ª����)
        //�浹�� ����
        //waypoint ���� �ٿ���, �ӵ� ���߱�
        //������ ������ ���ؼ�, ���� �ܷ� �����̸�, ����Ʈ(���̾�, ����, ��Ű�帶ũ) / 
    }


    void UpdateFollow()
    {
        //�ӵ� ��ȭ ��������� ���� / �ָ� ����
        //�ָ� ����
        //KHHWaypoint hitWaypoint = GetComponent<KHHWaypoint>();
        ////Vector3 dir= transform.position - hitWaypoint.GetComponentInParent<Vector3>();
        //if (Vector3.Distance(hitWaypoint.transform.position, transform.position) >=0.5f)
        //{
        //    speed = acceleration;
        //}
        ////������ ����
        //else
        //{
        //    speed = breakForce;
        //}
        //�̵�
        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, normalSpeed); //�̵�

    }
    //IEnumerator TowardPlayer()
    //{
    //    // 3�� ���� �����ϱ�
    
    //    yield return new WaitForSeconds(3);
    //}Z
    
    //ground ����
    bool Isground()
    {
        isGround = Physics.BoxCast(transform.position, groundBx * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f, groundLr);
        return isGround;
    }
    private void OnTriggerEnter(Collider other)
    {
        //waypoint�� ���ϱ�
        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();
        if(hitWaypoint == null) return;
        if (waypointIndex == hitWaypoint.waypointIndex) return;
        waypointIndex = hitWaypoint.waypointIndex;
        waypoint = hitWaypoint.NextPoint();

    }
    
}
