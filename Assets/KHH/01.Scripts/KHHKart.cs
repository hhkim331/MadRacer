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

    [Header("Move")]
    //speed
    int fowardBack = 1;
    public float normalSpeed = 20f;

    //boost
    float boostMultiply = 1.8f;
    float boostMax = 10f;
    float boostGauge = 10f;
    float boostUse = 2f;
    public Image gaugeBoostImage;
    public GameObject boostEffect;

    //drift
    public float driftMultifly = 1.5f;
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
    float fireTime = 0.1f;
    float fireDelay = 0.1f;

    bool fireLineOn = false;
    float fireLineTime = 0.0f;
    float fireLineDelay = 0.05f;

    public KHHLaser laser;
    public Transform weapon;
    public Transform firePos;
    LineRenderer fireLine;
    public ParticleSystem muzzleFlash;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fireLine = weapon.GetComponent<LineRenderer>();

        boostGauge = boostMax;
        gaugeBoostImage.fillAmount = 1f;
    }

    private void Update()
    {
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

        if (fireLineOn)
        {
            fireLineTime += Time.deltaTime;
            if (fireLineTime > fireLineDelay)
            {
                fireLineOn = false;
                fireLine.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMove();
        UpdateFire();
    }

    void UpdateMove()
    {
        //���󿡼��� �۵�
        if (IsGrounded())
        {
            float addSpeed = 0;
            MoveState moveState = GetMoveState();

            switch (moveState)
            {
                case MoveState.None:
                    //��������
                    carCollider.material = normalPM;
                    //ȸ������
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * KHHInput.instance.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    break;
                case MoveState.Accel:
                    //��������
                    carCollider.material = normalPM;
                    //ȸ������
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * KHHInput.instance.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    //����
                    addSpeed = normalSpeed - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    break;
                case MoveState.Drift:
                    //��������
                    carCollider.material = normalPM;
                    //ȸ������
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * KHHInput.instance.InputSteer * driftMultifly);
                        //rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    //����
                    addSpeed = driftSpeed - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    //�帮��Ʈ ����
                    boostGauge += Time.fixedDeltaTime * driftCharge;
                    if (boostGauge > boostMax)
                        boostGauge = boostMax;
                    gaugeBoostImage.fillAmount = boostGauge / boostMax;
                    break;
                case MoveState.Brake:
                    //��������
                    carCollider.material = brakePM;
                    //ȸ������
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        transform.Rotate(Vector3.up * KHHInput.instance.InputSteer);
                        rb.velocity = transform.forward * rb.velocity.magnitude * fowardBack;
                    }
                    break;
                case MoveState.Back:
                    //��������
                    carCollider.material = normalPM;
                    //����
                    addSpeed = backSpeed - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
                    addSpeed *= -1f;
                    break;
            }

            //�ν�Ʈ
            if (KHHInput.instance.InputBoost)
            {
                boostGauge -= Time.fixedDeltaTime * boostUse;
                if (boostGauge < 0)
                {
                    boostGauge = 0;
                    boostEffect.SetActive(false);
                }
                else
                {
                    addSpeed *= boostMultiply;
                    boostEffect.SetActive(true);
                }
                gaugeBoostImage.fillAmount = boostGauge / boostMax;
            }
            else
                boostEffect.SetActive(false);

            //�帮��Ʈ ����Ʈ
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
        weapon.LookAt(laser.HitPoint);
        if (KHHInput.instance.InputFire)
        {
            fireTime += Time.fixedDeltaTime;
            if (fireTime > fireDelay)
            {
                fireTime = 0;
                fireLineOn = true;
                fireLine.enabled = true;
                fireLineTime = 0f;
                fireLine.SetPosition(0, firePos.position);
                fireLine.SetPosition(1, laser.HitPoint);
                muzzleFlash.Play();
            }
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
