using UnityEngine;
using static KHHModel;

public class Enemy : MonoBehaviour
{
    public KHHModel model;
    public KHHModel.ModelType modelType = KHHModel.ModelType.Black;
    //EnemyEye�� ����Ʈ ù��° �����ϱ�
    //������ ����� ������ ����
    //������ �ı�
    //������ ��Ȱ..?
    #region ���� ��ȯ
    public enum EnemyState
    {
        Move,
        Booster,
        BulletAdd,
        Drift,
    }
    #endregion
    #region ����
    public EnemyState state;
    //EnemyEye enemyEye;
    WaypointFollow waypointFollow;

    public AudioSource sound;
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


    //����Ʈ ȿ����
    public GameObject driftREffect;
    public GameObject driftLEffect;
   
    //LineRenderer enemyAttackline;

    //public Transform muzzle;
    #endregion

    void Awake()
    {
        state = EnemyState.Move;
        boosterGauge = 0;

        driftLEffect.SetActive(false);
        driftREffect.SetActive(false);
        waypointFollow = GetComponent<WaypointFollow>();
    }

    private void Start()
    {
        model.Set(modelType);
        //���� ����
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
        //waypoint ��ȯ�� �ϸ�, 
    }

    private void UpdateBooster()
    {
        //���� ��ȯ
        //�����̴� �����϶�, ���ǵ� ����
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
        //�ν��� ������ ����.
        boosterGauge += Time.deltaTime;
        //EnemyEye���� ����Ʈ�� �νĵ� ������Ʈ�� ���� ���. << �̰� �����ε�, 
        //if (enemyEye.visibleTargets.Count > 0)
        //{
        //    //Attack ���·� �����ϱ�
        //    state = EnemyState.Attack;
        //}
        //����, item �� �ε����� ���
        //�ǰ� ������ ���,

        //���� �ν��� �������� �� á�� ���
        if (boosterGauge == 100)
        {
            //���� ��ȯ
            state = EnemyState.Booster;
        }
        else
        {
            waypointFollow.speed = waypointFollow.normalSpeed;
        }
    }

    
    ////���� �ҷ��ͼ�, bullet�̸� enemystye
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
