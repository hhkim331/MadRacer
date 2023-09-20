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
        Attack,
        //Item,
    }
    #endregion
    #region 변수
    public EnemyState state;
    EnemyEye enemyEye;
    WaypointFollow waypointFollow;
    EnemyHP enemyHP;



    float currentTime;
    public float fireTime;
    public GameObject bulletFactory;
    public Transform firePosition;

    public float boosterMaxGauge = 250;
    public float boosterGauge;
    public int bulletCount = 0;
    public int bulletMaxCount = 250;
    public float bulletTime = 0.1f;
    public float bulletDelay = 0.1f;


    //이펙트 효과들
    public GameObject driftREffect;
    public GameObject driftLEffect;
    public GameObject bulletEffect;

    LineRenderer enemyAttackline;

    public Transform muzzle;
    #endregion

    void Awake()
    {
        state = EnemyState.Move;
        boosterGauge = 0;
        bulletCount = 0;

        driftLEffect.SetActive(false);
        driftREffect.SetActive(false);
        enemyAttackline = GetComponent<LineRenderer>();
        enemyHP = GetComponent<EnemyHP>();
        enemyEye = GetComponent<EnemyEye>();

    }

    private void Start()
    {
        model.Set(modelType);
    }

    void Update()
    {
        //if (!KHHGameManager.instance.isStart) return;

        //EnemyState enemyState = GetEnemyState();
        boosterGauge += Time.deltaTime;
        switch (state)
        {
            case EnemyState.Move: UpdateMove(); break;
            case EnemyState.Attack: UpdateAttack(); break;
            //case EnemyState.Item: UpdateItem(); break;
        }
        //Vector3 dir = transform.position - enemyEye.visibleTargets[0].position;
    }






    private void UpdateMove()
    {
        //EnemyEye에서 리스트에 인식된 오브젝트가 있을 경우.
        if (enemyEye.visibleTargets.Count > 0)
        {

            //Attack 상태로 변경하기
            state = EnemyState.Attack;
        }
        //만약, item 과 부딪혔을 경우
        //피격 당했을 경우,
        //만약 부스터 게이지가 다 찼을 경우
        if (boosterGauge == 100)
        {
            //스피드 가속
            waypointFollow.speed = waypointFollow.acceleration;
        }
        else
        {
            waypointFollow.speed = waypointFollow.normalSpeed;
        }
    }


    private void UpdateAttack()
    {
        enemyAttackline.SetPosition(0, muzzle.position);
        print("l");
        if (enemyEye.visibleTargets.Count == 0) return;
        if (enemyEye.visibleTargets[0] != null)
        {
            print("3");
            // ray를 오브젝트 인식한 순서대로 쏨.
            Ray ray = new Ray(enemyEye.visibleTargets[0].transform.position, enemyEye.visibleTargets[0].transform.forward);
            RaycastHit hitInfo;
            //일정 시간을 더해서 시간이 되면,
            bulletTime += Time.deltaTime;

            if (Physics.Raycast(ray, out hitInfo, 25))
            {
                bulletMaxCount--;
                if (bulletTime > bulletDelay)
                {
                    if (bulletMaxCount == 0) return;
                    enemyAttackline.SetPosition(1, muzzle.position);
                    var bulletImpact = Instantiate(bulletEffect);
                    bulletImpact.transform.position = hitInfo.point;
                    bulletTime = 0;
                    enemyEye.visibleTargets[0].transform.GetComponentInParent<KHHHealth>().Hit(2);
                }
            }
            else
            {
                bulletTime = bulletDelay;
                enemyAttackline.SetPosition(1, ray.origin + ray.direction * 1000);
            }

        }
        else
        {
            state = EnemyState.Move;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //부스터 아이템에 부딪혔을때,
        if (collision.gameObject.CompareTag("Item"))
        {
            //부스터 게이지 채우기
            boosterGauge += 25;
        }
        //총알 아이템에 부딪혔을때
        else
        {
            //총알 채우기
            bulletCount += 25;
        }
    }
    IEnumerator startBooster()
    {
        //시간 더해서 일정시간이후에는 속도 감소
        yield return new WaitForSeconds(0.3f);
    }

}
