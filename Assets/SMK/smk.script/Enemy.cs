using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //EnemyEye�� ����Ʈ ù��° �����ϱ�
    //������ ����� ������ ����
    //������ �ı�
    //������ ��Ȱ..?


    public enum EnemyState
    {
        Move,
        Attack,
        MeleeAttack,
        Item,
        Die,
        Hit,
    }
    void Start()
    {
    }

    public EnemyState state;
    void Update()
    {
        //EnemyState enemyState = GetEnemyState();

        switch (state)
        {
            case EnemyState.Move: UpdateMove(); break;
            case EnemyState.Attack: UpdateAttack(); break;
            case EnemyState.MeleeAttack: UpdateMeleeAttack(); break;
            case EnemyState.Item: UpdateItem(); break;
            case EnemyState.Die: UpdateDie(); break;
            case EnemyState.Hit: UpdateHit(); break;
        }
        state = EnemyState.Move;
        //Vector3 dir = transform.position - enemyEye.visibleTargets[0].position;
    }
    public float enemyHP = 120;
    private void UpdateHit()
    {
        enemyHP -= 1;
        state = EnemyState.Attack;
        //�ǰ�
        //���� ������Ʈ�� tag..? layer�� ���̳� �÷��̾��, �ǰ�
        //��ֹ��̸�, 
        //waypoint ��ũ��Ʈ ����, �ڷ� �����̱�.


        //ü���� ������ die ���·� ��ȯ.
    }

    private void UpdateMeleeAttack()
    {
        //enemyEye.visibleTargets[0]�� 0.1m �� ���� ��� ����.
    }

    private void UpdateMove()
    {

        //if (dir )
        //{

        //    state = EnemyState.Attack;
        //}
    }

    private void UpdateDie()
    {
        //�� ������ �� ����, waypointFollow ���� ����
        //�ı��Ǵ� ����Ʈ�� �� �Ѽ� �����ֱ�
        //���� �� �ٽ� �� ���� , waypointFollow �ѱ�
    }

    private void UpdateItem()
    {
        throw new NotImplementedException();
    }

    EnemyEye enemyEye;

    float currentTime;
    public float fireTime;
    public GameObject bulletFactory;
    public Transform firePosition;
    private void UpdateAttack()
    {
        //�ð��� �帣�ٰ�. 
        currentTime += Time.deltaTime;
        //����ð��� firetiem�� �ʰ��ϸ�, 
        if (currentTime > fireTime)
        {
            //����ð��� 0���� �ʱ�ȭ�ϰ�
            currentTime = 0;
            //�Ѿ˰��忡�� �Ѿ��� �����
            var bullet = Instantiate<GameObject>(bulletFactory);
            bullet.transform.position = transform.position;
            //enemyEye���� �ν��� visibleTargets�� ù��°(0)�� ���ؼ�
            print(enemyEye.visibleTargets[0]);
            Vector3 dir = enemyEye.visibleTargets[0].position - firePosition.position;
            bullet.transform.forward = dir + Vector3.up * 10;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //�ǰ� ���·� ��ȯ
        if (collision != null)
        {

            state = EnemyState.Hit;

        }
    }
}
