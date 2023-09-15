using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    //Ray�� �ٲ㼭 �����ϱ�
    //���� ����̶� �ٸ�.

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
            // ray�� ������Ʈ �ν��� ������� ��.
            Ray ray = new Ray(enemyEye.visibleTargets[0].transform.position, enemyEye.visibleTargets[0].transform.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 5))
            {
                if (hitInfo.transform.gameObject.tag == "Player")
                {
                    //�� �¾Ҵٰ� �ҽ��ֱ�.
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
