using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHP : KHHHealth
{
    //�ǰ� ����
    //�״� ����


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


    //    //���� ������Ʈ�� tag..? layer�� ���̳� �÷��̾��, �ǰ�
    //    //��ֹ��̸�, 


    //}


    //private void UpdateDie()
    //{
    //    //waypointFollow ���� ����
    //    _waypointFollow.enabled = false;
    //    //�� ������ �� ����,
    //    _kart.gameObject.SetActive(false);
    //    //�ı��Ǵ� ����Ʈ�� �� �Ѽ� �����ֱ�
    //    destroyEffect.SetActive(true);
    //    destroyKart.gameObject.SetActive(true);
    //    //���� �� �ٽ� (move) ������+ HP ���� ���� , waypointFollow �ѱ�
    //    StartCoroutine(respawn(0.5f));
    //    //�μ����� ����
    //    destroyKart.gameObject.SetActive(false);
    //    _waypointFollow.enabled = true;
    //    _kart.gameObject.SetActive(true);


    //    // �̰� ������ ��� ����..

    //}
    ////������
    //IEnumerator respawn(float respawntime)
    //{
    //    yield return new WaitForSeconds(respawntime);
    //    enemyhp.hp = 100;
    //    //Instantiate()
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //�Ѿ˿� �ε�������
    //    if (collision.collider.gameObject.CompareTag("Respawn"))
    //    {
    //        //�ǰݻ��·� ����. �̰� ���� �θ���..
    //        UpdateHit(25, transform.position);
    //    }
    //}
}
