using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class KHHKart : MonoBehaviour
{
    KHHInput input;
    KHHWeapon weapon;
    KHHKartRank myKartRank;
    Rigidbody rb;

    public KHHModel model;
    public KHHModel.ModelType modelType = KHHModel.ModelType.Black;

    public enum MoveState
    {
        None,
        Accel,
        Drift,
        Brake,
        Back,
    }
    MoveState curMoveState = MoveState.None;
    MoveState newMoveState = MoveState.None;

    [Header("Move")]
    //speed
    public Transform handle;
    public Transform[] wheels;

    public float targetSpeed = 40f;
    int fowardBack = 1;
    public float normalSpeed = 40f;

    //boost
    public float boostMultiply = 1.8f;
    float boostMax = 100f;
    float boostGauge = 100f;
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
    float boostUse = 12f;
    public Image gaugeBoostImage;
    public GameObject boostEffect;

    //drift
    public float driftRotMultifly = 1.5f;
    public float driftSpeedMultifly = 0.7f;
    float driftCharge = 7f;
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

    //외부에 의한 부스터
    bool isBoost = false;
    float boostLeftTime = 0f;
    Coroutine coBoost;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<KHHInput>();
        weapon = GetComponent<KHHWeapon>();
        myKartRank = GetComponent<KHHKartRank>();
        myKartRank.IsMine = true;

        rb = GetComponent<Rigidbody>();

        model.Set(modelType);
        BoostGauge = boostMax;

        //wheel
        rb.centerOfMass = new Vector3(0, -1f, 0);
    }

    bool canStartBoost = true;
    float startBoostInputTime = 0f;

    private void Update()
    {
        handle.localRotation = Quaternion.Euler(15, 0, -input.InputSteer * 180f);
        if (myKartRank.isFinish) return;
        if (KHHGameManager.instance.isStart == false) return;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myKartRank.isFinish) return;
        if (KHHGameManager.instance.isStart == false)
        {
            //시작 부스터 판단을 위한 시간적립
            if (input.InputBoost)
                startBoostInputTime += Time.fixedDeltaTime;
            else
                startBoostInputTime = 0;
            return;
        }

        if (canStartBoost)
        {
            if (startBoostInputTime > 0 && startBoostInputTime < 0.5f)
                ApplyBoost(3f);
            canStartBoost = false;
        }

        UpdateMove();
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
            newMoveState = GetMoveState();
            switch (newMoveState)
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
                    if (!input.InputBoost)
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
            if (newMoveState == MoveState.Drift)
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

            //속도 적용
            if (!isBoost) rb.AddForce(transform.forward * addSpeed);

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

        UpdateSound();
        curMoveState = newMoveState;
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

    public void ApplyBoost(float boostTime)
    {
        isBoost = true;
        if (boostLeftTime < boostTime)
            boostLeftTime = boostTime;

        if (coBoost != null)
            StopCoroutine(coBoost);
        coBoost = StartCoroutine(CoBoost());
    }

    IEnumerator CoBoost()
    {
        while (boostLeftTime > 0)
        {
            if (!isBoost) //부스트 상태가 아니게 된 경우
            {
                boostLeftTime = 0;
                break;
            }

            float boostSpeed = targetSpeed * boostMultiply;
            if (rb.velocity.magnitude < boostSpeed * 0.5f)
                rb.velocity = transform.forward * boostSpeed * 0.5f;
            boostSpeed -= rb.velocity.magnitude;
            rb.AddForce(transform.forward * boostSpeed);

            boostLeftTime -= Time.deltaTime;
            yield return null;
        }

        boostLeftTime = 0;
        isBoost = false;
    }

    public void ApplyItem(Item.ItmeType itemType)
    {
        switch (itemType)
        {
            case Item.ItmeType.Bullet:
                weapon.BulletSupply();
                SoundManager.instance.PlaySFX("Reload");
                print("총알충전");
                break;
            case Item.ItmeType.Booster:
                BoostGauge = boostMax;
                print("부스터 충전");
                break;
            case Item.ItmeType.attack:
                weapon.SetWeapon();
                break;
            default:
                break;
        }
    }

    void UpdateSound()
    {
        if (input.InputBoost)
            SoundManager.instance.PlayEngine("EngineBoost");
        else
            SoundManager.instance.StopEngine("EngineBoost");

        switch (curMoveState)
        {
            case MoveState.Accel:
                SoundManager.instance.StopEngine("EngineIdle");
                SoundManager.instance.PlayEngine("EngineAccel");
                SoundManager.instance.StopEngine("EngineDrift");
                break;
            case MoveState.Drift:
                SoundManager.instance.StopEngine("EngineIdle");
                SoundManager.instance.PlayEngine("EngineAccel");
                SoundManager.instance.PlayEngine("EngineDrift");
                break;
            default:
                SoundManager.instance.PlayEngine("EngineIdle");
                SoundManager.instance.StopEngine("EngineAccel");
                SoundManager.instance.StopEngine("EngineDrift");
                break;
        }
    }
}