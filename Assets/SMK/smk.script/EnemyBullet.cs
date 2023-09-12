using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    Rigidbody rb;
    public float speed = 10f;
    bool ishit;

    Quaternion hitRotation;
    Vector3 hitPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

    }

    // Update is called once per frame
    void Update()
    {
        if (ishit)
        {
            transform.position = hitPosition;
            transform.rotation = hitRotation;
        }
        else
        {

            rb.transform.forward = rb.velocity.normalized;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);
        if (false == ishit)
        {
            ishit = true;
            hitRotation = transform.rotation;
            hitPosition = transform.position;

            rb.isKinematic = true;
            rb.useGravity = false;
            GetComponent<Collider>().enabled = false;

            StartCoroutine(CoFadeOut());
        }
    }
    IEnumerator CoFadeOut()
    {
        float alpha = -1;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                Color c = renderers[i].material.color;
                c.a = alpha;
                renderers[i].material.color = c;
            }
            alpha += Time.deltaTime;
            yield return 0;
        }
        Destroy(gameObject);

    }

}
