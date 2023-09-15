using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHP : KHHHealth
{
    //피격 상태
    //죽는 상태


    void Start()
    {
        
    }
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
}
