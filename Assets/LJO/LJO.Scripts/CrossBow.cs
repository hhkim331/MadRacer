using UnityEngine;
using System.Collections.Generic;

public class CrossBow : MonoBehaviour
{
    public GameObject bowFactory;
    public Transform inven;

    public Transform FirePosition;
    public Transform launchPad;  // �߻�� ��ġ
    public List<GameObject> magazine = new List<GameObject>();  // źâ�� �ִ� bullets
    public GameObject currentBullet;  // ���� �߻�뿡 �ִ� bullet

    private int bulletCount = 3;
    private float bowCreateTime;

    KHHKartRank kartRank;

    void Update()
    {
        if (bulletCount <= 0 && !currentBullet && magazine.Count == 0)
        {
            DestroyBow();
        }
        if (Input.GetMouseButtonDown(1)) // ���콺 ������ ��ư�� ������ ��
        {
            UpdateAttack();
        }
    }

    public void Set(KHHKartRank kartRank)
    {
        this.kartRank = kartRank;
        bowCreateTime = Time.time;
        GetComponent<Collider>().enabled = false;
        LoadBulletToLaunchPad(kartRank); // �ʱ� bullet ����
        for(int i=0;i< magazine.Count; i++)
        {
            magazine[i].GetComponent<PlayerBullet>().Set(kartRank);
        }
    }

    private void DestroyBow()
    {
        Destroy(gameObject);
    }

    

    private void LoadBulletToLaunchPad(KHHKartRank kartRank)
    {
        if (magazine.Count > 0)
        {
            // źâ�� ù ��° bullet�� �߻��� �̵�
            currentBullet = magazine[0];
            currentBullet.transform.position = launchPad.position;
            currentBullet.transform.rotation = launchPad.rotation;
            PlayerBullet bullet = currentBullet.GetComponent<PlayerBullet>();
            bullet.SetParent(transform);  // �θ� ����
            magazine.RemoveAt(0);  // źâ���� bullet ����
        }
    }
    public void UpdateAttack()
    {
        if (currentBullet)
        {
            currentBullet.GetComponent<PlayerBullet>().ReleaseParent(); // �θ� ���� ����
            currentBullet.GetComponent<PlayerBullet>().FireBullet(transform.forward); // �Ѿ��� �߻��ϴ� �Լ� ȣ��
            currentBullet = null;  // ���� �ʱ�ȭ
            bulletCount--;

            LoadBulletToLaunchPad(kartRank);  // �߻�뿡 bullet ����
        }
        Debug.Log("bulletCount:" + bulletCount);
    }
}
