using UnityEngine;
using UnityEngine.UI;

public class KHHKart : MonoBehaviour
{
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

    [Header("Wheel")]
    public WheelCollider[] wheels;
    public Transform[] wheelMeshes;
    public float power = 100f;
    public float steer = 30f;
    public float brake = 100f;

    [Header("Move")]
    //speed
    public Transform handle;

    int fowardBack = 1;
    public float normalSpeed = 20f;

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
    public float driftSpeed = 16f;
    float driftCharge = 3f;
    public GameObject driftRightEffect;
    public GameObject driftLeftEffect;

    //back
    public float backSpeed = 16f;

    [Header("Physics")]
    [SerializeField] Collider carCollider;
    [SerializeField] PhysicMaterial normalPM;
    [SerializeField] PhysicMaterial brakePM;

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
        rb = GetComponent<Rigidbody>();
        fireLine = weaponBarrel.GetComponent<LineRenderer>();

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

        handle.localRotation = Quaternion.Euler(15, 0, -KHHInput.instance.InputSteer * 180f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMove();
        UpdateFire();
    }

    void UpdateMove()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
            wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
        }
        for (int i = 0; i < 2; i++)
        {
            // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
            wheels[i].steerAngle = Input.GetAxis("Horizontal") * steer;
        }





        //지상에서만 작동
        if (IsGrounded())
        {
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
                        transform.Rotate(Vector3.up * KHHInput.instance.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    break;
                case MoveState.Accel:
                    //물리재질
                    carCollider.material = normalPM;
                    //회전보정
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * KHHInput.instance.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    //가속
                    addSpeed = normalSpeed - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    break;
                case MoveState.Drift:
                    //물리재질
                    carCollider.material = normalPM;
                    //회전보정
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * KHHInput.instance.InputSteer * driftRotMultifly);
                        //rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    //가속
                    addSpeed = driftSpeed - rb.velocity.magnitude;
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
                        transform.Rotate(Vector3.up * KHHInput.instance.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    break;
                case MoveState.Back:
                    //물리재질
                    carCollider.material = normalPM;
                    //가속
                    addSpeed = backSpeed - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    addSpeed *= -1f;
                    break;
            }

            //부스트
            if (KHHInput.instance.InputBoost && BoostGauge > 0)
            {
                boostEffect.SetActive(true);
                addSpeed *= boostMultiply;
                BoostGauge -= Time.fixedDeltaTime * boostUse;
            }
            else
                boostEffect.SetActive(false);

            //드리프트 이펙트
            if (moveState == MoveState.Drift)
            {
                if (KHHInput.instance.InputSteer > 0.1f)
                {
                    driftRightEffect.SetActive(true);
                    driftLeftEffect.SetActive(false);
                }
                else if (KHHInput.instance.InputSteer < -0.1f)
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
        }
    }

    MoveState GetMoveState()
    {
        if (KHHInput.instance.InputBrake)
        {
            if (KHHInput.instance.InputAccel)
            {
                if (Mathf.Abs(KHHInput.instance.InputSteer) > 0.1f)
                    return MoveState.Drift;
                else
                    return MoveState.Back;
            }
            else
                return MoveState.Brake;
        }
        else
        {
            if (KHHInput.instance.InputAccel)
                return MoveState.Accel;
            else
                return MoveState.None;
        }
    }

    void UpdateFire()
    {
        weaponBody.LookAt(laser.HitPoint);
        if (KHHInput.instance.InputFire && bulletCount > 0)
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

        if (KHHInput.instance.InputSub)
        {
            Debug.Log("Sub");
        }

        if (KHHInput.instance.InputShield)
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

    void OnDrawGizmos()
    {
        if (Physics.BoxCast(transform.position, groundBox * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.up * hit.distance);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + -transform.up * hit.distance, groundBox);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }
}
