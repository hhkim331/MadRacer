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
    private void Update()
    {
        if (enemyEye != null)
        {
            // ray를 오브젝트 인식한 순서대로 쏨.
            Ray ray = new Ray(enemyEye.visibleTargets[0].transform.position, enemyEye.visibleTargets[0].transform.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 5))
            {
                if (hitInfo.transform.gameObject.tag == "Player")
                {
                    //너 맞았다고 소식주기.
                    bulletEffect.gameObject.SetActive(true);
                    hitInfo.transform.GetComponent<Enemy>().UpdateHit(25, transform.position);
                    Destroy(hitInfo.transform.gameObject);
                    bulletEffect.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            else if (hitInfo.transform.gameObject)
            {
                bulletEffect.gameObject.SetActive(true);
                Destroy(hitInfo.transform.gameObject);
                bulletEffect.gameObject.SetActive(false);
                Destroy(gameObject);
            }

        }
    }
}
