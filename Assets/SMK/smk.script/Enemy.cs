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
        //MeleeAttack,
        Item,
        Die,
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
    //�ǰ�
    public void UpdateHit(int dmg, Vector3 origine)
    {
        if (enemyhp == null)
        {
            enemyhp = GetComponent<EnemyHP>();
        }
        enemyhp.hp -= dmg;
        if (enemyhp.hp > 0)
        {
            //�����·� ����
            state = EnemyState.Move;
        }
        else
        {
            //ü���� ������ die ���·� ��ȯ.
            state = EnemyState.Die;

        }
        //���� ������Ʈ�� tag..? layer�� ���̳� �÷��̾��, �ǰ�
        //��ֹ��̸�, 


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
        //waypointFollow ���� ����
        _waypointFollow.enabled = false;
        //�� ������ �� ����,
        _kart.gameObject.SetActive(false);
        //�ı��Ǵ� ����Ʈ�� �� �Ѽ� �����ֱ�
        destroyEffect.SetActive(true);
        destroyKart.gameObject.SetActive(true);
        //���� �� �ٽ� (move) ������+ HP ���� ���� , waypointFollow �ѱ�
        StartCoroutine(respawn(0.5f));
        //�μ����� ����
        destroyKart.gameObject.SetActive(false);
        _waypointFollow.enabled = true;
        _kart.gameObject.SetActive(true);


        // �̰� ������ ��� ����..

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
                    //�� �¾Ҵٰ� �ҽ��ֱ�.
                    hitInfo.transform.GetComponent<Enemy>().UpdateHit(25, transform.position);
                    //�̰� �����ؾ� ��..�÷��̾�� ���ʹ̵� ���� �ޱ�.^^
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
    //������
    IEnumerator respawn(float respawntime)
    {
        yield return new WaitForSeconds(respawntime);
        enemyhp.hp = 100;
        //Instantiate()
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�Ѿ˿� �ε�������
        if (collision.collider.gameObject.CompareTag("Respawn"))
        {
            //�ǰݻ��·� ����. �̰� ���� �θ���..
            UpdateHit(25, transform.position);
        }
    }
}
