using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KHHKart : MonoBehaviour
{
    KHHInput input;
    KHHWeapon weapon;
    KHHKartRank myKartRank;
    KHHPlayerHealth myHealth;
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
    float boostUse = 12f;
    public GameObject boostEffect;
    public ParticleSystem[] boostParticles;

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

    //�ܺο� ���� �ν���
    bool isBoost = false;
    float boostLeftTime = 0f;
    Coroutine coBoost;

    //����
    bool shieldActive = false;
    public bool ShieldActive
    {
        get { return shieldActive; }
    }
    float shieldUse = 12f;
    public Transform[] shieldObjs;

    //������
    float gaugeMax = 100f;
    float gauge = 100f;
    public float Gauge
    {
        get { return gauge; }
        set
        {
            gauge = value;
            if (gauge < 0) gauge = 0;
            if (gauge > gaugeMax) gauge = gaugeMax;
            gaugeImage.fillAmount = gauge / gaugeMax;
        }
    }
    public Image gaugeImage;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<KHHInput>();
        weapon = GetComponent<KHHWeapon>();
        myKartRank = GetComponent<KHHKartRank>();
        myHealth = GetComponent<KHHPlayerHealth>();
        myKartRank.IsMine = true;

        rb = GetComponent<Rigidbody>();

        if (PlayerPrefs.HasKey("SelectedModelType"))
        {
            modelType = (KHHModel.ModelType)PlayerPrefs.GetInt("SelectedModelType");
            Debug.Log("Loaded Model Type: " + modelType);
        }

        model.Set(modelType);
        Gauge = gaugeMax;

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

        //���������Ǵ�
        float dot = Vector3.Dot(transform.forward, rb.velocity);
        if (dot < 0) fowardBack = -1;
        else fowardBack = 1;

        //������ ����
        //���� ��ġ���� �ٴ����� ����ĳ��Ʈ
        Vector3 bottomNormal = Vector3.up;
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 10f))
            bottomNormal = hit.normal;

        //�ٴڰ��� ������ 45�� �̻��̸�
        if (Vector3.Angle(bottomNormal, transform.up) > 45f)
        {
            //�ٴڰ� ������ �������� Lerp����
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, bottomNormal), bottomNormal), Time.deltaTime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myHealth.IsDead) return;
        if (myKartRank.isFinish) return;
        if (KHHGameManager.instance.isStart == false)
        {
            //���� �ν��� �Ǵ��� ���� �ð�����
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
        UpdateShield();
    }

    void UpdateMove()
    {
        //���󿡼��� �۵�
        if (IsGrounded())
        {
            //�ν�Ʈ
            if (input.InputBoost)
            {
                if (Gauge > 0)
                {
                    boostEffect.SetActive(true);
                    foreach (var particle in boostParticles)
                        particle.Play();
                    targetSpeed = normalSpeed * boostMultiply;
                    Gauge -= Time.fixedDeltaTime * boostUse;
                }
                else
                {
                    boostEffect.SetActive(false);
                    foreach (var particle in boostParticles)
                        particle.Stop();
                    targetSpeed = normalSpeed;
                    KHHGameManager.instance.PlayerUI.NoEnergyBoost();
                }
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
                    //��������
                    carCollider.material = normalPM;
                    //ȸ������
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * input.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    break;
                case MoveState.Accel:
                    //��������
                    carCollider.material = normalPM;
                    //ȸ������
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * input.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    //����
                    addSpeed = targetSpeed - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    break;
                case MoveState.Drift:
                    //��������
                    carCollider.material = normalPM;
                    //ȸ������
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * input.InputSteer * driftRotMultifly);
                        //rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    //����
                    addSpeed = targetSpeed * driftSpeedMultifly - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    //�帮��Ʈ ����
                    if (!input.InputBoost)
                        Gauge += Time.fixedDeltaTime * driftCharge;
                    break;
                case MoveState.Brake:
                    //��������
                    carCollider.material = brakePM;
                    //ȸ������
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * input.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    break;
                case MoveState.Back:
                    //��������
                    carCollider.material = normalPM;
                    //����
                    addSpeed = targetSpeed * backSpeedMultifly - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    addSpeed *= -1f;
                    break;
            }

            addSpeed *= 1.5f;

            //�帮��Ʈ ����Ʈ
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

            //�ӵ� ����
            if (!isBoost) rb.AddForce(transform.forward * addSpeed);

            //�ٴڰ��� ����
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
            if (!isBoost) //�ν�Ʈ ���°� �ƴϰ� �� ���
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

    void UpdateShield()
    {
        //����
        if (input.InputShield)
        {
            if (Gauge > 0)
            {
                shieldActive = true;
                Gauge -= Time.fixedDeltaTime * shieldUse;
            }
            else
            {
                shieldActive = false;
                KHHGameManager.instance.PlayerUI.NoEnergyShield();
            }
        }
        else
            shieldActive = false;

        foreach (Transform shieldObj in shieldObjs)
            shieldObj.localScale = Vector3.MoveTowards(shieldObj.localScale, shieldActive ? Vector3.one : Vector3.zero, Time.fixedDeltaTime * 20);
    }

    public void ApplyItem(Item.ItemType itemType)
    {
        switch (itemType)
        {
            case Item.ItemType.Bullet:
                weapon.BulletSupply();
                SoundManager.instance.PlaySFX("Reload");
                break;
            case Item.ItemType.Booster:
                Gauge = gaugeMax;
                break;
            case Item.ItemType.attack:
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