using UnityEngine;

public class PlayerAttackTest : MonoBehaviour
{
    public KHHPlayerHealth playerHealth;

    float attackTimer = 0.0f;
    float attackDelay = 0.1f;

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackDelay)
        {
            if (Input.GetKey(KeyCode.A))
            {
                playerHealth.Hit(10);
                attackTimer = 0.0f;
            }
        }
    }
}
