using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarScript : MonoBehaviour
{
    public ItemDropManager dropManager;

    private Dictionary<Transform, float> lastDropTimeByPoint = new Dictionary<Transform, float>();
    public float dropCooldown = 60f;
    public GameObject bulletItemPrefab;
    public GameObject boosterItemPrefab;
    public GameObject attackItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();
        if (hitWaypoint != null && dropManager != null)
        {
            Item.ItmeType typeToSpawn;
            Transform targetDropPoint = dropManager.GetDropPointForWaypoint(hitWaypoint.waypointIndex, out typeToSpawn);

            if (targetDropPoint != null && CanDropItemAt(targetDropPoint))
            {
                SpawnItem(targetDropPoint, typeToSpawn);
                lastDropTimeByPoint[targetDropPoint] = Time.time;
            }
        }
    }
    bool CanDropItemAt(Transform dropPoint)
    {
        if (!lastDropTimeByPoint.ContainsKey(dropPoint))
            return true;

        return Time.time - lastDropTimeByPoint[dropPoint] > dropCooldown;
    }

    void SpawnItem(Transform dropPoint, Item.ItmeType typeToSpawn)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
