using UnityEngine;

public class CrossBow : MonoBehaviour
{
    public static CrossBow Instance;

    public GameObject bowFactory;
    public GameObject bow;
    public Transform inven;

    public Transform FirePosition;
    private int bulletCount = 3;
    private float bowCreateTime;

    // Start is called before the first frame update
    private void Awake()
    {
        bowCreateTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletCount <= 0)
        {
            DestroyBow();
        }
    }

    private void DestroyBow()
    {
        Destroy(gameObject);
    }

    public void UpdateAttack()
    {
        // var bullet = Instantiate(bowFactory);
        var bullet = Instantiate(bowFactory, FirePosition.position, FirePosition.rotation);
        bullet.transform.position = FirePosition.position;
        bulletCount--;

        Debug.Log("bulletCount:" +bulletCount);
    }
}