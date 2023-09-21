using UnityEngine;
using UnityEngine.UI;

public class KHHHealth : MonoBehaviour
{
    protected float health = 100;
    public float maxHealth = 100;
    public Image healthBar;

    bool isDead = false;
    float respawnTime = 0f;
    public float respawnDelay = 2.0f;

    public KHHKartRank kartRank;
    //»ç¸Á È¿°ú
    public GameObject model;
    public Rigidbody rb;

    public GameObject bulletItem;
    public GameObject explosionPrefab;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (kartRank.isFinish) return;

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

    public virtual void Hit(float damage, KHHKartRank kart)
    {
        if (kartRank.isFinish) return;

        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                Die();
                if (kartRank.IsMine)
                    KHHGameManager.instance.PlayerUI.EnemyKillPlayer(kart.name);
                else if (kart.IsMine)
                    KHHGameManager.instance.PlayerUI.PlayerKillEnemy(kartRank.name);
                else
                    KHHGameManager.instance.PlayerUI.EnemyKillEnemy(kart.name, kartRank.name);
            }
            healthBar.fillAmount = health / maxHealth;
        }
    }

    public virtual void Die()
    {
        isDead = true;
        model.SetActive(false);
        rb.isKinematic = true;

        SoundManager.instance.PlaySFXFromObject(transform.position, "Explosion");

        GameObject item = Instantiate(bulletItem, transform.position + Vector3.up, Quaternion.identity);

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
