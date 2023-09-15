using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using static KHHTarget;

public class KHHHealth : MonoBehaviour
{
    protected float health = 100;
    public float maxHealth = 100;
    public Image healthBar;

    bool isDead = false;
    float respawnTime = 0f;
    public float respawnDelay = 1.0f;

    //»ç¸Á È¿°ú
    public GameObject model;
    public Rigidbody rb;

    public GameObject bulletItem;
    public GameObject explosionPrefab;

    protected virtual void Update()
    {
        if (isDead)
        {
            respawnTime += Time.deltaTime;
            if (respawnTime > respawnDelay)
            {
                respawnTime = 0;
                Respawn();
            }
        }
    }

    public virtual void Hit(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health < 0)
            {
                health = 0;
                Die();
            }
            healthBar.fillAmount = health / maxHealth;
        }
    }

    public virtual void Die()
    {
        isDead = true;
        model.SetActive(false);
        rb.isKinematic = true;

        GameObject item = Instantiate(bulletItem, transform.position,Quaternion.identity);

        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position + Vector3.up;
        Destroy(explosion, 4);
    }

    public virtual void Respawn()
    {
        isDead = false;
        model.SetActive(true);
        rb.isKinematic = false;

        health = maxHealth;
        healthBar.fillAmount = 1;
    }
}
