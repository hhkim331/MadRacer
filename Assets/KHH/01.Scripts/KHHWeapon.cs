using TMPro;
using UnityEngine;

public class KHHWeapon : MonoBehaviour
{
    KHHInput input;

    //fire
    int bulletCount = 0;
    public int BulletCount
    {
        get { return bulletCount; }
        set
        {
            bulletCount = value;
            if (bulletCount < 0) bulletCount = 0;
            if (bulletCount > 250) bulletCount = 250;
            bulletText.text = bulletCount.ToString();
        }
    }
    public int bulletMax = 250;
    public TextMeshPro bulletText;

    float fireTime = 0.1f;
    float fireDelay = 0.1f;

    bool fireLineOn = false;
    float fireLineTime = 0.0f;
    float fireLineDelay = 0.05f;

    public KHHLaser laser;
    public Transform gunBody;
    public Transform gunBarrel;
    public Transform firePos;
    LineRenderer fireLine;
    public ParticleSystem muzzleFlash;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<KHHInput>();
        fireLine = firePos.GetComponent<LineRenderer>();
        BulletCount = 250;
    }

    // Update is called once per frame
    void Update()
    {
        bulletText.transform.LookAt(Camera.main.transform);
        if (KHHGameManager.instance.isStart == false) return;

        if (fireLineOn)
        {
            fireLineTime += Time.deltaTime;
            if (fireLineTime > fireLineDelay)
            {
                fireLineOn = false;
                fireLine.enabled = false;
            }
        }

        UpdateFire();
    }

    void UpdateFire()
    {
        gunBody.LookAt(laser.HitPoint);
        if (input.InputFire && bulletCount > 0)
        {
            fireTime += Time.deltaTime;
            if (fireTime > fireDelay)
            {
                BulletCount--;
                fireTime = 0;
                fireLineOn = true;
                fireLine.enabled = true;
                fireLineTime = 0f;
                fireLine.SetPosition(0, firePos.position);
                fireLine.SetPosition(1, laser.HitPoint);
                muzzleFlash.Play();
            }

            bulletText.transform.localScale = new Vector3(-0.05f, 0.05f, 0.05f) * laser.Distance;
            bulletText.transform.position = laser.HitPoint + Camera.main.transform.right * laser.Distance * 0.05f + Camera.main.transform.up * laser.Distance * 0.05f;
            gunBarrel.Rotate(Vector3.forward * 1000f * Time.deltaTime);
        }
        else
        {
            fireTime = fireDelay;

            bulletText.transform.localScale = new Vector3(-0.05f, 0.05f, 0.05f);
            bulletText.transform.localPosition = new Vector3(0, 0.1f, 0);
        }

        if (input.InputGrip)
        {
            Debug.Log("Sub");
        }

        if (input.InputShield)
        {
            Debug.Log("Shield");
        }
    }

    //ÃÑ¾Ë º¸±Þ
    public void BulletSupply()
    {
        BulletCount = bulletMax;
    }
}
