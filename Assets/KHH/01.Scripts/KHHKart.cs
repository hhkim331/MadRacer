using UnityEngine;
using UnityEngine.UI;

public class KHHKart : MonoBehaviour
{
    KHHInput input;
    KHHKartRank myKartRank;
    Rigidbody rb;

    //health
    public float health = 100f;

    public enum MoveState
    {
        None,
        Accel,
        Drift,
        Brake,
        Back,
    }

    [Header("Move")]
    //speed
    public Transform handle;
    public Transform[] wheels;

    public float targetSpeed = 40f;
    int fowardBack = 1;
    public float normalSpeed = 40f;

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
    public float driftSpeedMultifly = 0.7f;
    float driftCharge = 3f;
    public GameObject driftRightEffect;
    public GameObject driftLeftEffect;

    //back
    public float backSpeedMultifly = 0.7f;

    [Header("Physics")]
    [SerializeField] Collider carCollider;
    [SerializeField] PhysicMaterial normalPM;
    [SerializeField] PhysicMaterial brakePM;

    //ground
    //bool isGrounded = false;
    public LayerMask groundLayer;
    Vector3 groundNormal = Vector3.up;

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
        myKartRank = GetComponent<KHHKartRank>();
        rb = GetComponent<Rigidbody>();
        fireLine = firePos.GetComponent<LineRenderer>();

        BoostGauge = boostMax;

        //wheel
        rb.centerOfMass = new Vector3(0, -1f, 0);
    }

    private void Update()
    {
        //전진후진판단
        float dot = Vector3.Dot(transform.forward, rb.velocity);
        if (dot < 0) fowardBack = -1;
        else fowardBack = 1;

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
        if (input.InputReturn)
            ReturnTrack();
    }

    void UpdateMove()
    {
        //지상에서만 작동
        if (IsGrounded())
        {
            //부스트
            if (input.InputBoost && BoostGauge > 0)
            {
                boostEffect.SetActive(true);
                targetSpeed = normalSpeed * boostMultiply;
                BoostGauge -= Time.fixedDeltaTime * boostUse;
            }
            else
            {
                boostEffect.SetActive(false);
                targetSpeed = normalSpeed;
            }

            float addSpeed = 0;
            MoveState moveState = GetMoveState();

            switch (moveState)
            {
                case MoveState.None:
                    //물리재질
                    carCollider.material = normalPM;
                    //회전보정
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * input.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    break;
                case MoveState.Accel:
                    //물리재질
                    carCollider.material = normalPM;
                    //회전보정
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * input.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    //가속
                    addSpeed = targetSpeed - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    break;
                case MoveState.Drift:
                    //물리재질
                    carCollider.material = normalPM;
                    //회전보정
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * input.InputSteer * driftRotMultifly);
                        //rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    //가속
                    addSpeed = targetSpeed * driftSpeedMultifly - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    //드리프트 충전
                    BoostGauge += Time.fixedDeltaTime * driftCharge;
                    break;
                case MoveState.Brake:
                    //물리재질
                    carCollider.material = brakePM;
                    //회전보정
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * input.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    break;
                case MoveState.Back:
                    //물리재질
                    carCollider.material = normalPM;
                    //가속
                    addSpeed = targetSpeed * backSpeedMultifly - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    addSpeed *= -1f;
                    break;
            }

            addSpeed *= 1.5f;

            //드리프트 이펙트
            if (moveState == MoveState.Drift)
            {
                if (input.InputSteer > 0.1f)
                {
                    driftRightEffect.SetActive(true);
                    driftLeftEffect.SetActive(false);
                }
                else if (input.InputSteer < -0.1f)
                {
                    driftRightEffect.SetActive(false);
                    driftLeftEffect.SetActive(true);
                }
            }
            else
            {
                driftRightEffect.SetActive(false);
                driftLeftEffect.SetActive(false);
            }

            rb.AddForce(transform.forward * addSpeed);

            //바닥과의 각도
            Vector3 rawGroundNormal = Vector3.zero;
            foreach (var wheel in wheels)
                if (Physics.Raycast(wheel.position, -transform.up, out RaycastHit hit, 10f))
                    rawGroundNormal += hit.normal;
            rawGroundNormal.Normalize();
            groundNormal = Vector3.Slerp(groundNormal, rawGroundNormal, 10 * Time.fixedDeltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, groundNormal).normalized, groundNormal),
                Mathf.Clamp(Vector3.Dot(rb.velocity.normalized, groundNormal) * rb.velocity.magnitude * 10, 0.1f, 0.3f));

            transform.Rotate(rb.angularVelocity * Mathf.Rad2Deg * Time.fixedDeltaTime, Space.World);
        }
    }

    MoveState GetMoveState()
    {
        if (input.InputBrake)
        {
            if (input.InputAccel > 0.1f)
            {
                if (Mathf.Abs(input.InputSteer) > 0.1f)
                    return MoveState.Drift;
                else
                    return MoveState.Back;
            }
            else
                return MoveState.Brake;
        }
        else
        {
            if (input.InputAccel > 0.1f)
                return MoveState.Accel;
            else
                return MoveState.None;
        }
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
        foreach (var wheel in wheels)
        {
            if (Physics.Raycast(wheel.position, -transform.up, out RaycastHit hit, 1f))
                return true;
        }
        return false;
        //return Physics.BoxCast(transform.position + transform.forward * 0.3f, groundBox * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f, groundLayer);
    }

    void ReturnTrack()
    {
        Vector3 point = myKartRank.GetReturnTrackPoint();
        Physics.Raycast(point + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f, groundLayer);
        if (hit.collider != null)
            point = hit.point;
        transform.position = point;
        transform.forward = myKartRank.WayForward;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
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
