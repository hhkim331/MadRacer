using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static Item Instance;
    Rigidbody rb;
    Vector3 velocity;
    //public Vector3 gravity = new Vector3(0, -9.81f * 0.1f, 0);
    public enum ItmeType
    {
        Bullet,
        Booster,
        attack
        

    }
    public ItmeType itemType;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<TestScript>().ApplyItem(itemType);
            Destroy(gameObject);
        }
        if (other.CompareTag("Ground"))
        {
            rb.isKinematic = true;

        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.isKinematic = true;
            velocity = Vector3.zero;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //velocity += gravity * Time.deltaTime;
        //transform.position += velocity * Time.deltaTime;

    }
}
