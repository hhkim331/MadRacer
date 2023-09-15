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
    #region 상태 전환
    public enum EnemyState
    {
        Move,
        Attack,
        //MeleeAttack,
        Item,
        Die,
        Booster,
    }
    #endregion
    #region 변수
    public EnemyState state;
    EnemyEye enemyEye;
    WaypointFollow waypointFollow;



    float currentTime;
    public float fireTime;
    public GameObject bulletFactory;
    public Transform firePosition;

    public float boosterMaxGauge = 250;
    public float boosterGauge;

    private WaypointFollow _waypointFollow;
    public GameObject _kart;
    public GameObject destroyKart;

    //이펙트 효과들
    public GameObject driftREffect;
    public GameObject driftLEffect;
    public GameObject destroyEffect;
    #endregion

    void Awake()
    {
        state = EnemyState.Move;
        boosterGauge = 0;
        _waypointFollow = gameObject.GetComponent<WaypointFollow>();
        _kart = gameObject.GetComponent<GameObject>();
        destroyKart.SetActive(false);
        destroyEffect.SetActive(false);
        driftLEffect.SetActive(false);
        driftREffect.SetActive(false);


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
            case EnemyState: UpdateBooster(); break;
        }
        //Vector3 dir = transform.position - enemyEye.visibleTargets[0].position;
    }

    private void UpdateBooster()
    {
        waypointFollow.speed = waypointFollow.acceleration;
    }

    EnemyHP enemyhp;
    //피격
    public void UpdateHit(int dmg, Vector3 origine)
    {
        if (enemyhp == null)
        {
            enemyhp = GetComponent<EnemyHP>();
        }
        enemyhp.hp -= dmg;
        if (enemyhp.hp > 0)
        {
            //원상태로 복구
            state = EnemyState.Move;
        }
        else
        {
            //체력이 없으면 die 상태로 전환.
            state = EnemyState.Die;

        }
        //닿은 오브젝트의 tag..? layer가 적이나 플레이어면, 피격
        //장애물이면, 


    }


    private void UpdateMove()
    {
        //EnemyEye에서 리스트에 인식된 오브젝트가 있을 경우.
        if (EnemyEye.Instance.visibleTargets[0] != null)
        {
            //Attack 상태로 변경하기
            state = EnemyState.Attack;
        }

        //만약, item 과 부딪혔을 경우
        //피격 당했을 경우,
        //만약 부스터 게이지가 다 찼을 경우
        if (boosterGauge == 100)
        {
            state = EnemyState.Booster;
        }
    }
    private void UpdateDie()
    {
        //waypointFollow 상태 중지
        _waypointFollow.enabled = false;
        //본 상태의 모델 끄고,
        _kart.gameObject.SetActive(false);
        //파괴되는 이펙트와 모델 켜서 보여주기
        destroyEffect.SetActive(true);
        destroyKart.gameObject.SetActive(true);
        //몇초 후 다시 (move) 원상태+ HP 원상 복구 , waypointFollow 켜기
        StartCoroutine(respawn(0.5f));
        //부서진거 끄기
        destroyKart.gameObject.SetActive(false);
        _waypointFollow.enabled = true;
        _kart.gameObject.SetActive(true);


        // 이걸 정리를 어떻게 하지..

    }

    private void UpdateItem()
    {
        throw new NotImplementedException();
    }


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
            var bullet = Instantiate(bulletFactory);
            bullet.transform.position = firePosition.position;
            //enemyEye에서 인식한 visibleTargets의 첫번째(0)을 향해서
            print(EnemyEye.Instance.visibleTargets[0]);
            Vector3 dir = EnemyEye.Instance.visibleTargets[0].position - firePosition.position;
            bullet.transform.forward = dir + Vector3.up * 20;
        }
        state = EnemyState.Move;
    }
    //리스폰
    IEnumerator respawn(float respawntime)
    {
        yield return new WaitForSeconds(respawntime);
        enemyhp.hp = 100;
        //Instantiate()
    }

    private void OnCollisionEnter(Collision collision)
    {
        //총알에 부딪혔을때
        if (collision.collider.gameObject.CompareTag("bullet"))
        {
            //피격상태로 진입. 이거 어ㄸ허게 부르지..
            UpdateHit(25,transform.position);
        }
    }
}
