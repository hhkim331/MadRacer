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


    public Waypoint waypoint;//���ϴ� ��.
    Waypoint nextPoint;//���� ���� ��
    public float normalSpeed = 0.1f;//��� �ӵ�
    public float acceleration = 2f;//����
    public float breakForce = 0.01f;//����
    void Start()
    {

    }

    void Update()
    {
        if (Isground())
        {
            UpdateFollow();
        }
        //�̵������� enemy�� �չ���(z����)���� ����
        //transform.forward = waypoint.transform.forward;
        //(waypoint�� point ���� �Ÿ�)�� ������ waypoint�� �������� �������� �۰ų� ���� �� (= waypoint�� point���� �Ÿ��� ª����)
        //�浹�� ����
        //waypoint ���� �ٿ���, �ӵ� ���߱�
        //������ ������ ���ؼ�, ���� �ܷ� �����̸�, ����Ʈ(���̾�, ����, ��Ű�帶ũ) / 
    }

    public enum Enemystate { }


    void UpdateFollow()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, normalSpeed); //�̵�
        if ((waypoint.transform.position - transform.position).sqrMagnitude <= waypoint.radius * waypoint.radius)
        {

            //waypoint ����
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

    //ground ����
    bool Isground()
    {
        isGround = Physics.BoxCast(transform.position, groundBx * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f, groundLr);
        return isGround;
    }
}
