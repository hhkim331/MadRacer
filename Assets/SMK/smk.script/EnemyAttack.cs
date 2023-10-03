using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����

public class EnemyAttack : MonoBehaviour
{
    EnemyEye enemyEye;
    KHHKartRank kartRank;
    //EnemySound enemysound;

    public GameObject bulletEffect;
    public int bulletCount = 0;
    public int bulletMaxCount = 250;
    public float bulletTime = 0.1f;
    public float bulletDelay = 0.1f;

    public float fireTime;

    int MeleeAttackCount;

    LineRenderer enemyAttackline;
    public Transform muzzle;

    //enemy ���� 
    void Start()
    {
        bulletCount = bulletMaxCount;
        MeleeAttackCount = 0;
        enemyAttackline = GetComponent<LineRenderer>();
        enemyEye = GetComponent<EnemyEye>();
        kartRank = GetComponent<KHHKartRank>();
        //enemysound = GetComponent<EnemySound>();
    }

    void Update()
    {
        if (!KHHGameManager.instance.isStart) return;

        //�ν����� ���
        if (enemyEye.visibleTargets.Count > 0)
        {
            //Attack ���·� �����ϱ�
            Attack();
        }
    }
    void Attack()
    {
        //���� bulletCount�� ������ return;
        if (bulletCount <= 0) return;

        //���� bulletCount�� 0���� ũ��,(=bulletCount�� ������)
        if (bulletCount >= 0)
        {
            if (enemyEye.visibleTargets.Count == 0) return;
            //���̸� ����
            enemyAttackline.SetPosition(0, muzzle.position);
            //�������� �׸���


            if (enemyEye.visibleTargets[0] != null)
            {
                // ray�� ������Ʈ �ν��� �������
                Ray ray = new Ray(enemyEye.visibleTargets[0].transform.position, enemyEye.visibleTargets[0].transform.forward);
                RaycastHit hitInfo;
                //���� �ð��� ���ؼ� �ð��� �Ǹ�,
                bulletTime += Time.deltaTime;
                //�þ� �ȿ� �ִ� �Ϳ� ���̰� �¾������.
                if (Physics.Raycast(ray, out hitInfo, enemyEye.viewRadius))
                {
                    //�ð��� �ȵǾ����� ���ư�.
                    if (bulletTime <= bulletDelay) return;
                    //���ݽ� �Ѿ˼� ���̱�
                    bulletCount--;
                    //�ð���� �� �����ϱ�
                    if (bulletTime > bulletDelay)
                    {
                        enemyAttackline.SetPosition(1, muzzle.position);
                        //var bulletImpact = Instantiate(bulletEffect);
                        //bulletImpact.transform.position = hitInfo.point;

                        //���� �����ϱ�
                        EnemySound.Instance.Attack();

                        bulletTime = 0;
                        enemyEye.visibleTargets[0].transform.GetComponentInParent<KHHHealth>().Hit(2, kartRank);
                    }
                }
                else
                {
                    bulletTime = bulletDelay;
                    enemyAttackline.SetPosition(1, ray.origin + ray.direction * 1000);
                }

            }
        }

    }
    public void MeleeAttackAdd()
    {
        //�浹�� MeleeAttack()��
        MeleeAttackCount++;
        //���� �����ȿ� �ٸ� ������Ʈ�� ������ && MeleeAttackCount>=0 �̸� ������ȯ
        //�������� ���� MeleeAttackCount--;
    }
}
