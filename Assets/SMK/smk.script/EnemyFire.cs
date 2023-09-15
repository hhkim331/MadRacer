using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    //Ray로 바꿔서 공격하기
    //현재 방식이랑 다름.

    Rigidbody rb;
    public float speed = 10f;
    bool ishit;

    Quaternion hitRotation;
    Vector3 hitPosition;

    LineRenderer enemyLine;
    EnemyEye enemyEye;
    public GameObject bulletEffect;
    float bulletGauge = 250f;

    private void Start()
    {
        enemyEye = GetComponent<EnemyEye>();
        bulletEffect.SetActive(false);
    }
    //private void update()
    //{
    //    if (enemyeye != null)
    //    {
    //        // ray를 오브젝트 인식한 순서대로 쏨.
    //        ray ray = new ray(enemyeye.visibletargets[0].transform.position, enemyeye.visibletargets[0].transform.forward);
    //        raycasthit hitinfo;
    //        if (physics.raycast(ray, out hitinfo, 5))
    //        {
    //            if (hitinfo.transform.gameobject.tag == "player")
    //            {
    //                //너 맞았다고 소식주기.
    //                bulleteffect.gameobject.setactive(true);
    //                hitinfo.transform.getcomponent<enemy>().updatehit(25, transform.position);
    //                destroy(hitinfo.transform.gameobject);
    //                bulleteffect.gameobject.setactive(false);
    //                destroy(gameobject);
    //            }
    //        }
    //        else if (hitinfo.transform.gameobject)
    //        {
    //            bulleteffect.gameobject.setactive(true);
    //            destroy(hitinfo.transform.gameobject);
    //            bulleteffect.gameobject.setactive(false);
    //            destroy(gameobject);
    //        }

    //    }
    //}
}
