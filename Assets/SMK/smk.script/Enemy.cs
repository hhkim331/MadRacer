using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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



    float currentTime;
    public float fireTime;
    public GameObject bulletFactory;
    public Transform firePosition;

    public float boosterMaxGauge = 250;
    public float boosterGauge;

    private WaypointFollow _waypointFollow;
    public GameObject _kart;
    public GameObject destroyKart;

    //����Ʈ ȿ����
    public GameObject driftREffect;
    public GameObject driftLEffect;
    public GameObject destroyEffect;
    public GameObject bulletEffect;

    LineRenderer enemyAttackline;

    public Transform muzzle;
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
        enemyAttackline = GetComponent<LineRenderer>();

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
            case EnemyState:Booster: UpdateBooster(); break;
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
    

    private void UpdateItem()
    {
        throw new NotImplementedException();
    }


    private void UpdateAttack()
    {
        //�ð��� �帣�ٰ�. 
        //currentTime += Time.deltaTime;
        //if (currentTime > 0.2f)
        //{
        enemyAttackline.SetPosition(0, muzzle.position);
        print("l");
        if (enemyEye.visibleTargets[0] != null)
        {
            // ray�� ������Ʈ �ν��� ������� ��.
            Ray ray = new Ray(enemyEye.visibleTargets[0].transform.position, enemyEye.visibleTargets[0].transform.forward);
            RaycastHit hitInfo;
            enemyAttackline.SetPosition(1, muzzle.position);
            if (Physics.Raycast(ray, out hitInfo, 5))
            {
                var bulletImpact = Instantiate(bulletEffect);
                bulletImpact.transform.position = hitInfo.point;
                Destroy(bulletImpact, 0.5f);
                if (hitInfo.transform.gameObject.tag == "Player")
                {
                    print("rr");
                    ////�� �¾Ҵٰ� �ҽ��ֱ�.
                    //hitInfo.transform.GetComponent<Enemy>().UpdateHit(25, transform.position);
                    ////�̰� �����ؾ� ��..�÷��̾�� ���ʹ̵� ���� �ޱ�.^^
                    
                    Destroy(hitInfo.transform.gameObject);
                }
            }
            else
            {
                enemyAttackline.SetPosition(1, ray.origin + ray.direction * 1000);
            }

        }
        else
        {
            state = EnemyState.Move;
        }
    }
}
