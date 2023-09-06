using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KHHKart : MonoBehaviour
{
    Rigidbody rb;

    float speed = 10f;
    float normalspeed = 10f;
    float boostSpeed = 15f;

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
            if (KHHInput.instance.InputBoost)
                speed = boostSpeed;
            else
                speed = normalspeed;

            if (KHHInput.instance.InputAccel)
            {
                //회전보정
                if (KHHInput.instance.InputSteer != 0f)
                {
                    transform.Rotate(Vector3.up * KHHInput.instance.InputSteer);
                    rb.velocity = transform.forward * rb.velocity.magnitude;
                }

                //속도 추가가 가능한 경우에만
                float addSpeed = speed - rb.velocity.magnitude;
                if (addSpeed > 0f)
                    rb.AddForce(transform.forward * addSpeed);
            }
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
                //GameObject fireEffect = Instantiate(fireEffectPrefab, firePos.position, firePos.rotation);

                Vector3 targetPos = KHHInput.instance.InputTestTarget;
                if (targetPos == Vector3.zero)
                {
                    Vector3 screenMousePos = Input.mousePosition;
                    screenMousePos.z = 0.3f;
                    targetPos = Camera.main.ScreenPointToRay(screenMousePos).GetPoint(100);
                }

                fireLineOn = true;
                fireLine.enabled = true;
                fireLineTime = 0f;
                fireLine.SetPosition(0, firePos.position);
                fireLine.SetPosition(1, targetPos);
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
