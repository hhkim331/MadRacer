using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KHHKart : MonoBehaviour
{
    Rigidbody rb;

    float maxSpeed = 10f;

    //input
    bool inputAccel = false;
    bool inputBrake = false;
    float inputSteer = 0f;

    //ground
    bool isGrounded = false;
    public LayerMask groundLayer;
    public Vector3 groundBox = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputAccel = Input.GetKey(KeyCode.UpArrow);
        inputSteer = Input.GetAxis("Horizontal");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //지상에서만 작동
        if (inputAccel && IsGrounded())
        {
            //회전보정
            if (inputSteer != 0f)
            {
                transform.Rotate(Vector3.up * inputSteer);
                rb.velocity = transform.forward * rb.velocity.magnitude;
            }

            //속도 추가가 가능한 경우에만
            float addSpeed = maxSpeed - rb.velocity.magnitude;
            if (addSpeed > 0f)
                rb.AddForce(transform.forward * addSpeed);
        }
    }

    bool IsGrounded()
    {
        isGrounded = Physics.BoxCast(transform.position, groundBox * 0.5f, -transform.up, out RaycastHit hit, transform.rotation, 0.1f, groundLayer);
        return isGrounded;
    }

    void OnDrawGizmos()
    {
        if (Physics.BoxCast(transform.position, groundBox * 0.5f, Vector3.down, out RaycastHit hit, transform.rotation, 0.1f))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, groundBox);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }
}
