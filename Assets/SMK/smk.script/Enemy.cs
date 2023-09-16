using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //EnemyEye�� ����Ʈ ù��° �����ϱ�
    //������ ����� ������ ����
    //������ �ı�
    //������ ��Ȱ..?
    #region ���� ��ȯ
    public enum EnemyState
    {
        Move,
        Attack,
        Item,
        Booster,
    }
    #endregion
    #region ����
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


    //����Ʈ ȿ����
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

        driftLEffect.SetActive(false);
        driftREffect.SetActive(false);
        enemyAttackline = GetComponent<LineRenderer>();
        enemyHP = GetComponent<EnemyHP>();
        enemyEye = GetComponent<EnemyEye>();

    }

    void Update()
    {
        //EnemyState enemyState = GetEnemyState();
        boosterGauge += Time.deltaTime;
        switch (state)
        {
            case EnemyState.Move: UpdateMove(); break;
            case EnemyState.Attack: UpdateAttack(); break;
            case EnemyState.Item: UpdateItem(); break;
            case EnemyState: Booster: UpdateBooster(); break;
        }
        //Vector3 dir = transform.position - enemyEye.visibleTargets[0].position;
    }

    private void UpdateBooster()
    {
        waypointFollow.speed = waypointFollow.acceleration;
    }




    private void UpdateMove()
    {
        //EnemyEye���� ����Ʈ�� �νĵ� ������Ʈ�� ���� ���.
        if (enemyEye.visibleTargets.Count > 0)
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


    private void UpdateItem()
    {
        throw new NotImplementedException();
    }


    private void UpdateAttack()
    {
        enemyAttackline.SetPosition(0, muzzle.position);
        print("l");
        if (enemyEye.visibleTargets.Count == 0) return;
        if (enemyEye.visibleTargets[0] != null)
        {
            print("3");
            // ray�� ������Ʈ �ν��� ������� ��.
            Ray ray = new Ray(enemyEye.visibleTargets[0].transform.position, enemyEye.visibleTargets[0].transform.forward);
            RaycastHit hitInfo;
            //���� �ð��� ���ؼ� �ð��� �Ǹ�,
            bulletTime += Time.deltaTime; 

            if (Physics.Raycast(ray, out hitInfo, 25))
            {
                bulletMaxCount--;
                if (bulletTime > bulletDelay )
                {
                    if (bulletMaxCount == 0) return;
                    enemyAttackline.SetPosition(1, muzzle.position);
                    var bulletImpact = Instantiate(bulletEffect);
                    bulletImpact.transform.position = hitInfo.point;
                    bulletTime = 0;
                    print("1");
                    enemyEye.visibleTargets[0].transform.GetComponentInParent<EnemyHP>().Hit(25);
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

}
