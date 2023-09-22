using UnityEngine;
using System.Collections.Generic;

public class CrossBow : MonoBehaviour
{
    public static CrossBow Instance;

    public GameObject bowFactory;
    public Transform inven;

    public Transform FirePosition;
    public Transform launchPad;  // 발사대 위치
    public List<GameObject> magazine = new List<GameObject>();  // 탄창에 있는 bullets
    public GameObject currentBullet;  // 현재 발사대에 있는 bullet

    private int bulletCount = 3;
    private float bowCreateTime;

    private void Awake()
    {
        Instance = this;
        bowCreateTime = Time.time;
        LoadBulletToLaunchPad(); // 초기 bullet 장전
    }

    void Update()
    {
        if (bulletCount <= 0 && !currentBullet && magazine.Count == 0)
        {
            DestroyBow();
        }
        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼을 눌렀을 때
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
            // 탄창의 첫 번째 bullet을 발사대로 이동
            currentBullet = magazine[0];
            currentBullet.transform.position = launchPad.position;
            currentBullet.transform.rotation = launchPad.rotation;
            currentBullet.GetComponent<PlayerBullet>().SetParent(transform);  // 부모 설정
            magazine.RemoveAt(0);  // 탄창에서 bullet 제거
        }
    }
    public void UpdateAttack()
    {
        if (currentBullet)
        {
            currentBullet.GetComponent<PlayerBullet>().ReleaseParent(); // 부모 설정 해제
            currentBullet.GetComponent<PlayerBullet>().FireBullet(); // 총알을 발사하는 함수 호출
            currentBullet = null;  // 참조 초기화
            bulletCount--;

            LoadBulletToLaunchPad();  // 발사대에 bullet 장전
        }
        Debug.Log("bulletCount:" + bulletCount);
    }
}
