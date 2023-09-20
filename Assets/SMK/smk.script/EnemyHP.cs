using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHP : KHHHealth
{
    //�ǰ� ȿ��
    float hitTime = 0.0f;
    public float hitDuration = 0.5f;
    float intensity = 0.0f;
    public float maxIntensity = 0.3f;
    Coroutine hitEffect;

    WaypointFollow _waypointFollow;
    Enemy enemy;
    //�״� ����

    //public GameObject hitEffect;
    protected override void Start()
    {
        _waypointFollow = GetComponent<WaypointFollow>();
        enemy = GetComponent<Enemy>();
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        //���� �ð��� ������ UI ��Ȱ��ȭ

        if (health > 0)
        {
            hitTime = 0;
            if (hitEffect != null)
                StopCoroutine(hitEffect);
            //�ǰ� �޾������ uI Ȱ��ȭ
            //����Ʈ Ȱ��ȭ->��Ȱ��ȭ.
            hitEffect = StartCoroutine(HitEffect());
        }
    }

    public override void Die()
    {
        base.Die();
        enemy.enabled = false;
        _waypointFollow.enabled = false;

    }
    public override void Respawn()
    {
        base.Respawn();
        hitTime = 0;
        intensity = 0;
        enemy.enabled = true;
        //������ �� ��� uI Ȱ��ȭ
        //���� �ð��� ������ UI ��Ȱ��ȭ
        _waypointFollow.enabled = true;
    }
    #region
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
    #endregion
    IEnumerator HitEffect()
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
