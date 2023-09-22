using TMPro;
using UnityEngine;
using static KHHTarget;

public class KHHWeapon : MonoBehaviour
{
    KHHInput input;
    KHHKartRank kartRank;

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

    public Transform gripRight;

    public KHHLaser laser;
    public Transform gunBody;
    public Transform gunBarrel;
    public Transform firePos;
    LineRenderer fireLine;
    public ParticleSystem muzzleFlash;
    public GameObject metalEffect;
    public GameObject sandEffect;
    public GameObject stoneEffect;

    bool gripSubWeapon = false;
    GameObject subWeapon;
    public GameObject bow;
    public Transform inven;
    CrossBow crossBow;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<KHHInput>();
        kartRank = GetComponent<KHHKartRank>();
        fireLine = firePos.GetComponent<LineRenderer>();
        BulletCount = 250;
    }

    public Animator saw;
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

        if (gripSubWeapon)
        {
            UpdateSubWeapon();
        }
        else
        {
            UpdateFire();
            if (subWeapon != null && input.InputGrip)
                GripWeapon();
        }
    }

    void UpdateFire()
    {
        gunBody.LookAt(laser.HitPoint);
        if (input.InputFire)
        {
            if (bulletCount > 0)
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
                    SoundManager.instance.PlaySFX("Fire");

                    KHHHealth health = laser.hitObj.GetComponentInParent<KHHHealth>();
                    if (health != null)
                        health.Hit(10, kartRank);

                    KHHTarget target = laser.hitObj.GetComponentInParent<KHHTarget>();
                    if (target != null)
                    {
                        if (target.hitType == KHHTarget.HitType.Metal)
                        {
                            Instantiate(metalEffect, laser.HitPoint, Quaternion.LookRotation(laser.HitNormal));
                        }
                        else if (target.hitType == KHHTarget.HitType.Sand)
                        {
                            Instantiate(sandEffect, laser.HitPoint, Quaternion.LookRotation(laser.HitNormal));
                        }
                        else if (target.hitType == KHHTarget.HitType.Stone)
                        {
                            Instantiate(stoneEffect, laser.HitPoint, Quaternion.LookRotation(laser.HitNormal));
                        }
                    }
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
                KHHGameManager.instance.PlayerUI.NoBullet();
            }
        }
        else
        {
            fireTime = fireDelay; 
            bulletText.transform.localScale = new Vector3(-0.05f, 0.05f, 0.05f);
            bulletText.transform.localPosition = new Vector3(0, 0.1f, 0);
        }

        if (input.InputShield)
        {
            Debug.Log("Shield");
        }
    }

    bool subFire = false;
    void UpdateSubWeapon()
    {
        if (crossBow == null)
        {
            gripSubWeapon = false;
            return;
        }

        if(input.InputFire)
        {
            if(subFire==false)
            {
                subFire = true;
                crossBow.UpdateAttack();
            }
        }
        else
        {
            subFire = false;
        }
    }

    //총알 보급
    public void BulletSupply()
    {
        BulletCount = bulletMax;
    }

    public void SetWeapon()
    {
        if (subWeapon == null)
        {
            subWeapon = Instantiate(bow, inven);
            KHHGameManager.instance.PlayerUI.SubWeaponReady();
        }
    }

    void GripWeapon()
    {
        float distance = Vector3.Distance(gripRight.position, subWeapon.transform.position);
        if (distance < 1)
        {
            gripSubWeapon = true;
            subWeapon.transform.SetParent(gripRight);
            subWeapon.transform.localPosition = Vector3.zero;
            subWeapon.transform.localRotation = Quaternion.identity;
            subWeapon.transform.localScale = Vector3.one * 0.1f;
            crossBow = subWeapon.GetComponent<CrossBow>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //적 레이어
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            saw.SetTrigger("Active");
            KHHHealth health = other.GetComponentInParent<KHHHealth>();
            if (health != null)
                health.Hit(999, kartRank);
        }
    }
}
