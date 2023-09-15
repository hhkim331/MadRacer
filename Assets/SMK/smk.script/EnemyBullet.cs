using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
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
                if (hitInfo.transform.gameObject.tag == "Kart")
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
    #region
    //void Start()
    //{
    //    //Ray ray = GetComponent<Ray>();
    //    rb = GetComponent<Rigidbody>();
    //    rb.velocity = transform.forward * speed;

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (ishit)
    //    {
    //        transform.position = hitPosition;
    //        transform.rotation = hitRotation;
    //    }
    //    else
    //    {

    //        rb.transform.forward = rb.velocity.normalized;
    //    }
    //}
    //float alpha = -1;
    //Renderer[] renderers = GetComponentsInChildren<Renderer>();
    //for (float time = 0; time < 1; time += Time.deltaTime)
    //{
    //    for (int i = 0; i < renderers.Length; i++)
    //    {
    //        Color c = renderers[i].material.color;
    //        c.a = alpha;
    //        renderers[i].material.color = c;
    //    }
    //    alpha += Time.deltaTime;
    //    yield return 0;
    //}
    #endregion
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //Destroy(gameObject);
    //    if (false == ishit)
    //    {
    //        ishit = true;
    //        hitRotation = transform.rotation;
    //        hitPosition = transform.position;

    //        rb.isKinematic = true;
    //        rb.useGravity = false;
    //        GetComponent<Collider>().enabled = false;

    //        StartCoroutine(CoFadeOut());
    //    }
    //}
    //IEnumerator CoFadeOut()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    Destroy(gameObject);
    //}

}
