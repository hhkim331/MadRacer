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
        //���� ����
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
        //���� ��ȯ
        //�����̴� �����϶�, ���ǵ� ����
        if (boosterGauge >= 0)
        {
            waypointFollow.speed += 5 * Time.deltaTime;
            waypointFollow.speed = Mathf.Clamp(waypointFollow.speed, 0, waypointFollow.acceleration);
        }
        boosterGauge -= Time.deltaTime;
        if (boosterGauge <= 0)
        {
            state = EnemyState.Move;
        }
    }

    public void UpdateBulletAdd()
    {
        enemyAttack.bulletCount += 25;
    }
    //private void UpdateMove()
    //{
    //    //�ν��� ������ ����.
    //    boosterGauge += Time.deltaTime;

    //    //���� �ν��� �������� �� á�� ���
    //    if (boosterGauge == 100)
    //    {
    //        //���� ��ȯ
    //        state = EnemyState.Booster;
    //    }
    //}

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
