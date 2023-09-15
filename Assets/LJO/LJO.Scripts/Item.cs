using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static Item Instance;
    Rigidbody rb;
    Vector3 velocity;
    public Transform playerCar;  // 플레이어 차량의 Transform
    public float spawnInterval = 1.0f; // 예: 5초마다 아이템 생성
    public float dropCooldown = 60f; // 60초 쿨다운
   // private HashSet<Transform> usedDropPoints = new HashSet<Transform>();

    //public Vector3 gravity = new Vector3(0, -9.81f * 0.1f, 0);
    public enum ItmeType
    {
        Bullet,
        Booster,
        attack
        

    }
    public ItmeType itemType;
    private Dictionary<Transform, float> lastDropTimeByPoint = new Dictionary<Transform, float>();
    [System.Serializable]
    public class DropPointInfo
    {
        public Transform dropPoint;
        public ItmeType itemType;
    }
    public List<DropPointInfo> dropPointInfos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCar == null) // 에디터에서 할당되지 않은 경우
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerCar = playerObj.transform;
            }
        }
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<KHHKart>().ApplyItem(itemType);
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
    public float rotationSpeed = 20f; // 초당 20도 회전
    void Update()
    {
        //velocity += gravity * Time.deltaTime;
        //transform.position += velocity * Time.deltaTime;
        // Y축을 중심으로 아이템 박스 회전
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        CheckForItemDrop();


    }
    public List<Transform>dropPoints = new List<Transform>(); //드롭 포인트 리스트
  

    public float distanceInFrontOfCar = 100f;  // 차량 전방의 거리
    public float heightAboveCar = 50f;  // 차량 위의 높이

    Transform GetNearestDropPoint(out ItmeType nearestItemType)
    {
        float minDistance = float.MaxValue;
        Transform nearestDropPoint = null;
        nearestItemType = itemType; // default

        foreach (var info in dropPointInfos)
        {
            float distance = Vector3.Distance(playerCar.position, info.dropPoint.position);
            if (distance < minDistance && (!lastDropTimeByPoint.ContainsKey(info.dropPoint) || Time.time - lastDropTimeByPoint[info.dropPoint] > dropCooldown))
            {
                minDistance = distance;
                nearestDropPoint = info.dropPoint;
                nearestItemType = info.itemType;
            }
        }

        return nearestDropPoint;
    }
    public GameObject itemPrefab;  // 아이템 프리팹
    void CheckForItemDrop()
    {
        ItmeType nearestItemType;
        Transform nearestDropPoint = GetNearestDropPoint(out nearestItemType);

        if (nearestDropPoint != null)  // 여기서 추가된 조건문!
        {
            float distanceToDropPoint = Vector3.Distance(playerCar.position, nearestDropPoint.position);

            if (distanceToDropPoint < distanceInFrontOfCar)
            {
                SpawnItem(nearestDropPoint, nearestItemType);
                lastDropTimeByPoint[nearestDropPoint] = Time.time;
            }
        }
    }

    bool CanDropItemAt(Transform dropPoint)
    {
        if (!lastDropTimeByPoint.ContainsKey(dropPoint))
            return true;

        return Time.time - lastDropTimeByPoint[dropPoint] > dropCooldown;
    }
    void SpawnItem(Transform dropPoint, ItmeType typeToSpawn)
    {
        Vector3 spawnPosition = dropPoint.position + Vector3.up * heightAboveCar;
        GameObject droppedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        droppedItem.GetComponent<Item>().itemType = typeToSpawn;
    }
    //Vector3 GetDropPosition()
    //{
    //    Transform nearestDropPoint = GetNearestDropPoint();
    //    if (nearestDropPoint)
    //    {
    //        usedDropPoints.Add(nearestDropPoint);
    //        if(usedDropPoints.Count == dropPoints.Count)
    //        {
    //            usedDropPoints.Clear();
    //        }
    //        return nearestDropPoint.position + Vector3.up * heightAboveCar;
    //    }

    //    return Vector3.zero;
    //}
    //Vector3 GetDropPosition()
    //{
    //    Transform nearestDropPoint = GetNearestDropPoint();
    //    Vector3 dropPosition = nearestDropPoint.position + Vector3.up * heightAboveCar;
    //    return dropPosition;
    //}
   

   

}
