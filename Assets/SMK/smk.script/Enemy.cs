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
        //MeleeAttack,
        Item,
        Die,
        Hit,
        Booster,
    }
    public float enemyHP = 120;
    public EnemyState state;
    EnemyEye enemyEye;
    WaypointFollow waypointFollow;

    float currentTime;
    public float fireTime;
    public GameObject bulletFactory;
    public Transform firePosition;

    public float boosterMaxGauge = 250;
    public float boosterGauge;
    void Start()
    {
        state = EnemyState.Move;
        boosterGauge = 0;
    }

    void Update()
    {
        //EnemyState enemyState = GetEnemyState();
        boosterGauge += Time.deltaTime;
        switch (state)
        {
            case EnemyState.Move: UpdateMove(); break;
            case EnemyState.Attack: UpdateAttack(); break;
            //case EnemyState.MeleeAttack: UpdateMeleeAttack(); break;
            case EnemyState.Item: UpdateItem(); break;
            case EnemyState.Die: UpdateDie(); break;
            case EnemyState.Hit: UpdateHit(); break;
            case EnemyState: UpdateBooster(); break;
        }
        //Vector3 dir = transform.position - enemyEye.visibleTargets[0].position;
    }

    private void UpdateBooster()
    {
        waypointFollow.speed = waypointFollow.acceleration;
    }

    //�ǰ�
    private void UpdateHit()
    {
        enemyHP -= 1;
        if (enemyHP > 0)
        {
            //waypoint ��ũ��Ʈ ��� ���߰�, �ڷ� �����̱�.

            state = EnemyState.Move;
        }
        else
        {
            state = EnemyState.Die;

        }
        //���� ������Ʈ�� tag..? layer�� ���̳� �÷��̾��, �ǰ�
        //��ֹ��̸�, 


        //ü���� ������ die ���·� ��ȯ.
    }

    private void UpdateMeleeAttack()
    {
        //enemyEye.visibleTargets[0]�� 0.1m �� ���� ��� ����.
    }

    private void UpdateMove()
    {
        //EnemyEye���� ����Ʈ�� �νĵ� ������Ʈ�� ���� ���.
        if (EnemyEye.Instance.visibleTargets[0] != null)
        {
            //Attack ���·� �����ϱ�
            state = EnemyState.Attack;
        }

        //����, item �� �ε����� ���
        //�ǰ� ������ ���,
        //���� �ν��� �������� �� á�� ���
        if (boosterGauge == 100)
        {
            state = EnemyState.Booster;
        }
    }

    private void UpdateDie()
    {

        //�� ������ �� ����, waypointFollow ���� ����
        //�ı��Ǵ� ����Ʈ�� �� �Ѽ� �����ֱ�
        //���� �� �ٽ� (move) ������+ HP ���� ���� , waypointFollow �ѱ�
    }

    private void UpdateItem()
    {
        throw new NotImplementedException();
    }


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
            var bullet = Instantiate(bulletFactory);
            bullet.transform.position = firePosition.position;
            //enemyEye���� �ν��� visibleTargets�� ù��°(0)�� ���ؼ�
            print(EnemyEye.Instance.visibleTargets[0]);
            Vector3 dir = EnemyEye.Instance.visibleTargets[0].position - firePosition.position;
            bullet.transform.forward = dir + Vector3.up * 20;
        }
        state = EnemyState.Move;
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //�ǰ� ���·� ��ȯ
    //    if (collision != null)
    //    {

    //        state = EnemyState.Hit;

    //    }
    //}

    private void OnTriggerEnter()
    {

    }
}
