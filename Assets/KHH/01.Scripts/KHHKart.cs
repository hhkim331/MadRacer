using UnityEngine;

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

    //speed
    int fowardBack = 1;
    float normalSpeed = 14f;

    //boost
    float boostMultiply = 1.5f;
    float boostGauge = 100f;

    //drift
    float driftSpeed = 12f;
    float driftCharge = 10f;

    //back
    float backSpeed = 12f;

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
    public Transform firePos;
    LineRenderer fireLine;
    public GameObject fireEffectPrefab;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        fireLine = firePos.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (fireLineOn)
        {
            fireLineTime += Time.deltaTime;
            if (fireLineTime > fireLineDelay)
            {
                fireLineOn = false;
                fireLine.enabled = false;
            }
        }

        //전진후진판단
        float dot = Vector3.Dot(transform.forward, rb.velocity);
        if (dot < 0) fowardBack = -1;
        else fowardBack = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMove();
        UpdateFire();
    }

    void UpdateMove()
    {
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
                        transform.Rotate(Vector3.up * KHHInput.instance.InputSteer);
                        rb.velocity = (rb.velocity.normalized + transform.forward).normalized * rb.velocity.magnitude * fowardBack;
                    }
                    //가속
                    addSpeed = driftSpeed - rb.velocity.magnitude;
                    if (addSpeed < 0) addSpeed = 0;
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

            if (KHHInput.instance.InputBoost)
                addSpeed *= boostMultiply;
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
