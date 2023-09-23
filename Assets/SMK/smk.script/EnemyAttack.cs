using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격

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

    //enemy 공격 
    void Start()
    {
        bulletCount = 0;
        MeleeAttackCount = 0;
        enemyAttackline = GetComponent<LineRenderer>();
        enemyEye = GetComponent<EnemyEye>();
        kartRank = GetComponent<KHHKartRank>();
        //enemysound = GetComponent<EnemySound>();
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

        //만약 bulletCount가 0보다 크면,(=bulletCount가 있으면)
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
                //시야 안에 있는 것에 레이가 맞았을경우.
                if (Physics.Raycast(ray, out hitInfo, enemyEye.viewRadius))
                {
                    //시간이 안되었으면 돌아가.
                    if (bulletTime <= bulletDelay) return;
                    //공격시 총알수 줄이기
                    bulletCount--;
                    //시간경과 후 공격하기
                    if (bulletTime > bulletDelay)
                    {
                        enemyAttackline.SetPosition(1, muzzle.position);
                        //var bulletImpact = Instantiate(bulletEffect);
                        //bulletImpact.transform.position = hitInfo.point;

                        //사운드 실행하기
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
    public void BulletAdd()
    {
        bulletCount += 25;
    }
    public void MeleeAttackAdd()
    {
        //충돌시 MeleeAttack()에
        MeleeAttackCount++;
        //일정 범위안에 다른 오브젝트가 있으면 && MeleeAttackCount>=0 이면 상태전환
        //근접공격 시행 MeleeAttackCount--;
    }
}
