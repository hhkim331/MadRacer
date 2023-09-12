using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //EnemyEye의 리스트 첫번째 공격하기
    //아이템 섭취시 몇초후 시행
    //죽으면 파괴
    //몇초후 부활..?


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

    private void UpdateHit()
    {
        //피격
        //닿은 오브젝트의 tag..? layer가 적이나 플레이어면, 피격
        //장애물이면, 
        //waypoint 스크립트 중지, 뒤로 움직이기.
        

        //체력이 없으면 die 상태로 전환.
    }

    private void UpdateMeleeAttack()
    {
        //enemyEye.visibleTargets[0]가 0.1m 에 있을 경우 시행.
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
        //본 상태의 모델 끄고, waypointFollow 상태 중지
        //파괴되는 이펙트와 모델 켜서 보여주기
        //몇초 후 다시 본 상태 , waypointFollow 켜기
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
        //시간이 흐르다가. 
        currentTime += Time.deltaTime;
        //현재시간이 firetiem을 초과하면, 
        if (currentTime > fireTime)
        {
            //현재시간을 0으로 초기화하고
            currentTime = 0;
            //총알공장에서 총알을 만들고
            var bullet = Instantiate<GameObject>(bulletFactory);
            bullet.transform.position = transform.position;
            //enemyEye에서 인식한 visibleTargets의 첫번째(0)을 향해서
            print(enemyEye.visibleTargets[0]);
            Vector3 dir = enemyEye.visibleTargets[0].position - firePosition.position;
            bullet.transform.forward = dir + Vector3.up * 10;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //피격 상태로 변환
        state = EnemyState.Hit;
    }
}
