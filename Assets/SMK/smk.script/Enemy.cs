using System;
using System.Collections;
using UnityEngine;
using static KHHModel;

public class Enemy : MonoBehaviour
{
    public KHHModel model;
    public KHHModel.ModelType modelType = KHHModel.ModelType.Black;
    //EnemyEye의 리스트 첫번째 공격하기
    //아이템 섭취시 몇초후 시행
    //죽으면 파괴
    //몇초후 부활..?
    #region 상태 전환
    public enum EnemyState
    {
        Move,
        Booster,
        BulletAdd,
        Drift,
    }
    #endregion
    #region 변수
    public EnemyState state;
    //EnemyEye enemyEye;
    WaypointFollow waypointFollow;
    EnemyHP enemyHP;
    EnemyAttack enemyAttack;


    float currentTime;
    //public float fireTime;
    //public GameObject bulletFactory;
    //public Transform firePosition;

    public float boosterGauge;
    public float boosterMaxGauge = 250;
    //public int bulletCount = 0;
    //public int bulletMaxCount = 250;
    //public float bulletTime = 0.1f;
    //public float bulletDelay = 0.1f;


    //이펙트 효과들
    public GameObject driftREffect;
    public GameObject driftLEffect;

    //LineRenderer enemyAttackline;

    //public Transform muzzle;
    #endregion

    void Awake()
    {
        state = EnemyState.Move;
        boosterGauge = 0;
        //bulletCount = 0;

        driftLEffect.SetActive(false);
        driftREffect.SetActive(false);
        //enemyAttackline = GetComponent<LineRenderer>();
        enemyHP = GetComponent<EnemyHP>();
        //enemyEye = GetComponent<EnemyEye>();



    }

    private void Start()
    {
        model.Set(modelType);
        //증가 제한
        boosterGauge = Mathf.Clamp(boosterGauge, 0, 250);

    }

    void Update()
    {
        //if (!KHHGameManager.instance.isStart) return;

        //EnemyState enemyState = GetEnemyState();
        switch (state)
        {
            case EnemyState.Move: UpdateMove(); break;
            case EnemyState.Booster: UpdateBooster(); break;
            case EnemyState.Drift: UpdateDrift(); break;
        }
        //Vector3 dir = transform.position - enemyEye.visibleTargets[0].position;
    }

    private void UpdateDrift()
    {
        //waypoint 전환을 하면, 
    }

    private void UpdateBooster()
    {
        //상태 변환
        //움직이는 상태일때, 스피드 가속
        waypointFollow.speed = waypointFollow.acceleration;
        if (boosterGauge >=0)
        {

        }
        boosterGauge -= Time.deltaTime;
        if (boosterGauge <= 0)
        {
            state = EnemyState.Move;
        }
    }

    private void UpdateMove()
    {
        //부스터 게이지 증가.
        boosterGauge += Time.deltaTime;
        //EnemyEye에서 리스트에 인식된 오브젝트가 있을 경우. << 이게 문제인데, 
        //if (enemyEye.visibleTargets.Count > 0)
        //{
        //    //Attack 상태로 변경하기
        //    state = EnemyState.Attack;
        //}
        //만약, item 과 부딪혔을 경우
        //피격 당했을 경우,

        //만약 부스터 게이지가 다 찼을 경우
        if (boosterGauge == 100)
        {
            //상태 변환
            state = EnemyState.Booster;
        }
        else
        {
            waypointFollow.speed = waypointFollow.normalSpeed;
        }
    }

    
    //상태 불러와서, bullet이면 enemystye
    public void EnemyItem(Item.ItmeType itemType)
    {
        switch (itemType)
        {
            case Item.ItmeType.Bullet:
                enemyAttack.BulletAdd();
                break;
            case Item.ItmeType.Booster:
                boosterGauge = boosterMaxGauge;
                break;
            case Item.ItmeType.attack:
                enemyAttack.MeleeAttack();
                break;
            default:
                break;
        }
    }

}
