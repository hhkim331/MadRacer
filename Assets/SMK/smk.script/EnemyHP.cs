using System.Collections;
using UnityEngine;



public class EnemyHP : KHHHealth
{
    float hitTime = 0.0f;
    public float hitDuration = 0.5f;
    float intensity = 0.0f;
    public float maxIntensity = 0.3f;
    Coroutine hitEffect;

    WaypointFollow _waypointFollow;
    Enemy enemy;

    public WaypointFollow waypointFollow;
    public GameObject UI;

    protected override void Start()
    {
        _waypointFollow = GetComponent<WaypointFollow>();
        enemy = GetComponent<Enemy>();
    }



    public override void Hit(float damage, KHHKartRank kart)
    {

        base.Hit(damage, kart);


        if (health > 0)
        {
            //�ǰ� �޾������+ uI Ȱ��ȭ
            //if (.CompareTag("Enemy") == )
                UI.SetActive(true);

            hitTime = 0;
            if (hitEffect != null)
                StopCoroutine(hitEffect);

            //����Ʈ Ȱ��ȭ->��Ȱ��ȭ.
            hitEffect = StartCoroutine(HitEffect());
            //�ǰ� ���� ���� ��� ��Ȱ��ȭ.
            StartCoroutine(EndUI());
        }
    }

    public override void Die()
    {
        base.Die();
        waypointFollow.speed = 0;
        //���� ��� uI Ȱ��ȭ
        UI.SetActive(true);
        //���� ��� ���ӵ� �ʱ�ȭ
        _waypointFollow.speed = 0;

        enemy.enabled = false;
        _waypointFollow.enabled = false;
        StartCoroutine(EndUI());


    }
    public override void Respawn()
    {
        //������ �� ��� uI Ȱ��ȭ
        UI.SetActive(true);

        base.Respawn();
        hitTime = 0;
        intensity = 0;
        enemy.enabled = true;
        _waypointFollow.enabled = true;
        //���� �ð��� ������ UI ��Ȱ��ȭ
        StartCoroutine(EndUI());

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
    
    //���� �ð��� ������ UI ��Ȱ��ȭ
    IEnumerator EndUI()
    {
        yield return new WaitForSeconds(0.1f);
        UI.SetActive(false);
    }
}
