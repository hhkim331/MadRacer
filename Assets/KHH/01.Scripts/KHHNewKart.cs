using UnityEngine;
using UnityEngine.UI;

public class KHHNewKart : MonoBehaviour
{
    KHHInput input;
    Rigidbody rb;

    //health
    public float health = 100f;

    bool isBrake = false;
    bool isDrift = false;

    [Header("Wheel")]
    public WheelCollider[] wheels;
    public Transform[] wheelMeshes;
    public float power = 100f;
    public float steer = 30f;
    public float brake = 100f;

    [Header("Move")]
    //speed
    public Transform handle;

    //boost
    public float boostMultiply = 1.8f;
    float boostMax = 10f;
    float boostGauge = 10f;

    public float BoostGauge
    {
        get { return boostGauge; }
        set
        {
            boostGauge = value;
            if (boostGauge < 0) boostGauge = 0;
            if (boostGauge > boostMax) boostGauge = boostMax;
            gaugeBoostImage.fillAmount = boostGauge / boostMax;
        }
    }
    float boostUse = 2f;
    public Image gaugeBoostImage;
    public GameObject boostEffect;

    //drift
    public float driftRotMultifly = 1.5f;
    //public float driftAdditional = 0.3f;
    float driftCharge = 3f;
    public GameObject driftRightEffect;
    public GameObject driftLeftEffect;

    //ground
    //bool isGrounded = false;
    public LayerMask groundLayer;
    public Vector3 groundBox = Vector3.zero;

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


        }
    }
    public int bulletMax = 250;

    float fireTime = 0.1f;
    float fireDelay = 0.1f;

    bool fireLineOn = false;
    float fireLineTime = 0.0f;
    float fireLineDelay = 0.05f;

    public KHHLaser laser;
    public Transform weaponBody;
    public Transform weaponBarrel;
    public Transform firePos;
    LineRenderer fireLine;
    public ParticleSystem muzzleFlash;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<KHHInput>();
        rb = GetComponent<Rigidbody>();
        fireLine = weaponBarrel.GetComponent<LineRenderer>();

        BoostGauge = boostMax;

        //wheel
        rb.centerOfMass = new Vector3(0, -1f, 0);
    }

    private void Update()
    {
        //뒤집힘 방지
        //현재 위치에서 바닥으로 레이캐스트
        Vector3 bottomNormal = Vector3.up;
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 10f))
            bottomNormal = hit.normal;

        //바닥과의 각도가 45도 이상이면
        if (Vector3.Angle(bottomNormal, transform.up) > 45f)
        {
            //바닥과 수직인 방향으로 Lerp보정
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, bottomNormal), bottomNormal), Time.deltaTime);
        }

        if (fireLineOn)
        {
            fireLineTime += Time.deltaTime;
            if (fireLineTime > fireLineDelay)
            {
                fireLineOn = false;
                fireLine.enabled = false;
            }
        }

        handle.localRotation = Quaternion.Euler(15, 0, -input.InputSteer * 180f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMove();
        UpdateFire();
    }

    void UpdateMove()
    {
        float addPower = power;

        //지상에서만 작동
        if (IsGrounded())
        {
            CheckState();
            Drift();

            //브레이크
            for (int i = 0; i < 2; i++)
                wheels[i].brakeTorque = isBrake ? brake : 0;

            //부스트
            if (input.InputBoost && BoostGauge > 0)
            {
                boostEffect.SetActive(true);
                addPower *= boostMultiply;
                BoostGauge -= Time.fixedDeltaTime * boostUse;
            }
            else
                boostEffect.SetActive(false);


            for (int i = 0; i < wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
                wheels[i].motorTorque = input.InputAccel * addPower;
            }
            for (int i = 0; i < 2; i++)
            {
                // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
                wheels[i].steerAngle = input.InputSteer * steer;
            }
        }
    }

    void CheckState()
    {
        isDrift = false;
        isBrake = false;
        if (input.InputBrake)
        {
            if (input.InputAccel > 0.1f && Mathf.Abs(input.InputSteer) > 0.1f)
                isDrift = true;
            else
                isBrake = true;
        }
    }

    void Drift()
    {
        for (int i = 0; i < 2; i++)
        {
            WheelFrictionCurve forwardFriction = wheels[i].forwardFriction;
            forwardFriction.stiffness = isDrift ? 0.5f : 1f;
            wheels[i].forwardFriction = forwardFriction;

            WheelFrictionCurve sidewaysFriction = wheels[i].sidewaysFriction;
            sidewaysFriction.stiffness = isDrift ? 0.5f : 1f;
            wheels[i].sidewaysFriction = sidewaysFriction;
        }

        if (isDrift)
            BoostGauge += Time.fixedDeltaTime * driftCharge;

        //드리프트 이펙트
        driftRightEffect.SetActive(isDrift && input.InputSteer > 0.1f);
        driftLeftEffect.SetActive(isDrift && input.InputSteer < -0.1f);
    }

    void UpdateFire()
    {
        weaponBody.LookAt(laser.HitPoint);
        if (input.InputFire && bulletCount > 0)
        {
            fireTime += Time.fixedDeltaTime;
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

            weaponBarrel.Rotate(Vector3.forward * 1000f * Time.fixedDeltaTime);
        }
        else
        {
            fireTime = fireDelay;
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

    bool IsGrounded()
    {
        return Physics.BoxCast(transform.position, groundBox * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f, groundLayer);
    }

    public void ApplyItem(Item.ItmeType itemType)
    {
        switch (itemType)
        {
            case Item.ItmeType.Bullet:
                BulletCount = 250;
                print("총알충전");
                break;
            case Item.ItmeType.Booster:
                BoostGauge = boostMax;
                print("부스터 충전");
                break;
            default:
                break;
        }
    }
}
