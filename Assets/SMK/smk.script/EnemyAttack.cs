using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : MonoBehaviour
{
    EnemyEye enemyEye;
    KHHKartRank kartRank;

    public GameObject bulletEffect;
    public int bulletCount = 0;
    public int bulletMaxCount = 250;
    public float bulletTime = 0.1f;
    public float bulletDelay = 0.1f;

    public float fireTime;
    //public GameObject bulletFactory;
    //public Transform firePosition;


    LineRenderer enemyAttackline;
    public Transform muzzle;


    //enemy 공격 
    void Start()
    {
        bulletCount = 0;
        enemyAttackline = GetComponent<LineRenderer>();
        enemyEye = GetComponent<EnemyEye>();
        kartRank = GetComponent<KHHKartRank>();

    }

    void Update()
    {
        //if (!KHHGameManager.instance.isStart) return;

        //인식했을 경우
        if (enemyEye.visibleTargets.Count > 0)
        {
            //Attack 상태로 변경하기
            Attack();
        }
    }
    void Attack()
    {
        //만약 bulletCount가 없으면 return;
        if (bulletCount <= 0) return;

        //만약 bullet 카운트가 0보다 크면,( bulletCount 가 있으면)
        if (bulletCount >= 0)
        {
            if (enemyEye.visibleTargets.Count == 0) return;
            //레이를 쏴서
            enemyAttackline.SetPosition(0, muzzle.position);
            //레이저를 그리고

            if (enemyEye.visibleTargets[0] != null)
            {
                // ray를 오브젝트 인식한 순서대로
                Ray ray = new Ray(enemyEye.visibleTargets[0].transform.position, enemyEye.visibleTargets[0].transform.forward);
                RaycastHit hitInfo;
                //일정 시간을 더해서 시간이 되면,
                bulletTime += Time.deltaTime;
                //인식했을시
                if (Physics.Raycast(ray, out hitInfo, 25))
                {
                    //시간이 안되었으면 돌아가.
                    if (bulletTime <= bulletDelay) return;
                    //공격시 총알수 줄이기
                    bulletCount--;
                    //시간경과 후 공격하기
                    if (bulletTime > bulletDelay)
                    {
                        //if (bulletMaxCount == 0) return;
                        enemyAttackline.SetPosition(1, muzzle.position);
                        //var bulletImpact = Instantiate(bulletEffect);
                        //bulletImpact.transform.position = hitInfo.point;
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
    public void BulletAdd()
    {
        bulletCount += 25;
    }
    public void MeleeAttack()
    {
        //근접공격 시행.
        //옆동네 근접공격좀 보고 비슷하게 따라하기.
    }
}
