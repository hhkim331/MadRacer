using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class EnemyHP : KHHHealth
{
    //피격 효과
    float hitTime = 0.0f;
    public float hitDuration = 0.5f;
    float intensity = 0.0f;
    public float maxIntensity = 0.3f;
    Coroutine coHitEffect;
    //죽는 상태

    void Start()
    {
        
    }
    private override void Enemyhit(float damage)
    {
        base.Hit(damage);
        if (health > 0)
        {
            hitTime = 0;
            if (coHitEffect != null)
                StopCoroutine(coHitEffect);
            coHitEffect = StartCoroutine(CoHitEffect());
        }
    }

    private override void Enemydie()
    {
        base.Die();
    }
    public override void EnemyRespawn()
    {
        base.Respawn();
        hitTime = 0;
        intensity = 0;
    }
    #region
    //public void UpdateHit(int dmg, Vector3 origine)
    //{
    //    if (health == null)
    //    {
    //        health = GetComponent<EnemyHP>();
    //    }
    //    health.hp -= dmg;


    //    //닿은 오브젝트의 tag..? layer가 적이나 플레이어면, 피격
    //    //장애물이면, 


    //}


    //private void UpdateDie()
    //{
    //    //waypointFollow 상태 중지
    //    _waypointFollow.enabled = false;
    //    //본 상태의 모델 끄고,
    //    _kart.gameObject.SetActive(false);
    //    //파괴되는 이펙트와 모델 켜서 보여주기
    //    destroyEffect.SetActive(true);
    //    destroyKart.gameObject.SetActive(true);
    //    //몇초 후 다시 (move) 원상태+ HP 원상 복구 , waypointFollow 켜기
    //    StartCoroutine(respawn(0.5f));
    //    //부서진거 끄기
    //    destroyKart.gameObject.SetActive(false);
    //    _waypointFollow.enabled = true;
    //    _kart.gameObject.SetActive(true);


    //    // 이걸 정리를 어떻게 하지..

    //}
    ////리스폰
    //IEnumerator respawn(float respawntime)
    //{
    //    yield return new WaitForSeconds(respawntime);
    //    enemyhp.hp = 100;
    //    //Instantiate()
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //총알에 부딪혔을때
    //    if (collision.collider.gameObject.CompareTag("Respawn"))
    //    {
    //        //피격상태로 진입. 이거 어ㄸ허게 부르지..
    //        UpdateHit(25, transform.position);
    //    }
    //}
    #endregion
    IEnumerator CoHitEffect()
    {
        while (hitTime < hitDuration)
        {
            hitTime += Time.deltaTime;
            intensity += Time.deltaTime * maxIntensity / hitDuration;
            if (intensity > maxIntensity) intensity = maxIntensity;
            yield return null;
        }

        while (hitTime > 0)
        {
            hitTime -= Time.deltaTime;
            intensity -= Time.deltaTime * maxIntensity / hitDuration;
            if (intensity < 0) intensity = 0;
            yield return null;
        }
    }
}
