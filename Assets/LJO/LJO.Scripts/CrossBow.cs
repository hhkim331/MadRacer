using UnityEngine;
using System.Collections.Generic;

public class CrossBow : MonoBehaviour
{
    public static CrossBow Instance;

    public GameObject bowFactory;
    public Transform inven;

    public Transform FirePosition;
    public Transform launchPad;  // �߻�� ��ġ
    public List<GameObject> magazine = new List<GameObject>();  // źâ�� �ִ� bullets
    public GameObject currentBullet;  // ���� �߻�뿡 �ִ� bullet

    private int bulletCount = 3;
    private float bowCreateTime;

    private void Awake()
    {
        Instance = this;
        bowCreateTime = Time.time;
        LoadBulletToLaunchPad(); // �ʱ� bullet ����
    }

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

    private void DestroyBow()
    {
        Destroy(gameObject);
    }

    

    private void LoadBulletToLaunchPad()
    {
        if (magazine.Count > 0)
        {
            // źâ�� ù ��° bullet�� �߻��� �̵�
            currentBullet = magazine[0];
            currentBullet.transform.position = launchPad.position;
            currentBullet.transform.rotation = launchPad.rotation;
            currentBullet.GetComponent<PlayerBullet>().SetParent(transform);  // �θ� ����
            magazine.RemoveAt(0);  // źâ���� bullet ����
        }
    }
    public void UpdateAttack()
    {
        if (currentBullet)
        {
            currentBullet.GetComponent<PlayerBullet>().ReleaseParent(); // �θ� ���� ����
            currentBullet.GetComponent<PlayerBullet>().FireBullet(); // �Ѿ��� �߻��ϴ� �Լ� ȣ��
            currentBullet = null;  // ���� �ʱ�ȭ
            bulletCount--;

            LoadBulletToLaunchPad();  // �߻�뿡 bullet ����
        }
        Debug.Log("bulletCount:" + bulletCount);
    }
}
