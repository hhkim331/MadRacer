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
    EnemyAttack enemyAttack;
    public AudioSource sound;
    float currentTime;
    //public float fireTime;
    //public GameObject bulletFactory;
    //public Transform firePosition;

    public float boosterGauge;
    public float boosterMaxGauge = 250;


    //LineRenderer enemyAttackline;

    //public Transform muzzle;
    #endregion

    void Awake()
    {
        state = EnemyState.Move;
        boosterGauge = 0;
        waypointFollow = GetComponent<WaypointFollow>();

    }

    private void Start()
    {
        model.Set(modelType);
        //증가 제한
        boosterGauge = Mathf.Clamp(boosterGauge, 0, 250);

    }


    public void item(Item.ItemType itemType)
    {
        switch (itemType)
        {
            //case EnemyState.Move: UpdateMove(); break;
            case Item.ItemType.Booster: UpdateBooster(); EnemySound.Instance.Booster(); break;
            case Item.ItemType.Bullet: UpdateBulletAdd(); break;
        }
    }
    private void UpdateBooster()
    {
        //상태 변환
        //움직이는 상태일때, 스피드 가속
        if (boosterGauge >= 0)
        {
            waypointFollow.max = waypointFollow.acceleration;
        }
        boosterGauge -= Time.deltaTime;
        if (boosterGauge <= 0)
        {
            waypointFollow.max = waypointFollow.normalSpeed;
        }
    }

    public void UpdateBulletAdd()
    {
        enemyAttack.bulletCount += 25;
    }
    //private void UpdateMove()
    //{
    //    //부스터 게이지 증가.
    //    boosterGauge += Time.deltaTime;

    //    //만약 부스터 게이지가 다 찼을 경우
    //    if (boosterGauge == 100)
    //    {
    //        //상태 변환
    //        state = EnemyState.Booster;
    //    }
    //}

    ////상태 불러와서, bullet이면 enemystye
    //public void EnemyItem(Item.ItmeType itemType)
    //{
    //    switch (itemType)
    //    {
    //        case Item.ItmeType.Bullet:
    //            enemyAttack.BulletAdd();
    //            break;
    //        case Item.ItmeType.Booster:
    //            boosterGauge = boosterMaxGauge;
    //            break;
    //        case Item.ItmeType.attack:
    //            enemyAttack.MeleeAttackAdd();
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
