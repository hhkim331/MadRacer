using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;
using static KHHWaypoint;

[System.Serializable]
public class WaypointItemSpawnInfo
{
    public int waypointIndex; // 웨이포인트 인덱스
    public ItemType itemType; // 생성할 아이템의 종류
    public Transform[] spawnPoints; // 해당 인덱스에서 아이템을 생성할 위치
}

public enum ItemType
{
    Item1,
    Item2,
    Item3
}

public class KHHKartRank : MonoBehaviour
{
    public bool isFinish = false;
    public int rank = 0;
    public int lap = 0;
    public int finalLap = 2;
    public int wayPointIndex = 0;
    public float wayPercent = 0;
    public GameObject itemPrefab;
    bool isMine = false;
    public bool IsMine { get { return isMine; } set { isMine = value; } }
   //public Dictionary<int, GameObject> waypointItemMapping = new Dictionary<int, GameObject>();
   public List<WaypointItemSpawnInfo> waypointItemSpawnInfos = new List<WaypointItemSpawnInfo>();

    int checkPointCount = 0;

    public KHHWaypoint prevWaypoint;
    public KHHWaypoint nextWaypoint;
    public Vector3 WayForward { get { return (nextWaypoint.transform.parent.position - prevWaypoint.transform.parent.position).normalized; } }

    //public Transform[] itemSpawnPoints;

    //private IEnumerator Start()
    //{
    //    yield return new WaitForSeconds(1f);
    //    isFinish = true;
    //    KHHGameManager.instance.GameEnd();
    //}

    // Update is called once per frame
    void Update()
    {
        wayPercent = Vector3.Distance(prevWaypoint.transform.parent.position, transform.position) /
            (Vector3.Distance(prevWaypoint.transform.parent.position, transform.position) +
            Vector3.Distance(nextWaypoint.transform.parent.position, transform.position));
    }
    public GameObject[] itemPrefabs;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("dsdsds");
        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();
        Debug.Log(hitWaypoint);
        if (hitWaypoint == null) return;
        if (wayPointIndex == hitWaypoint.waypointIndex) return;
        //if (isMine)
        //{
        //    // 웨이포인트를 지날 때 아이템 프리팹 인스턴스화
        //    foreach (Transform spawnPoint in itemSpawnPoints)
        //    {
        //        Debug.Log("cani");
        //        Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
        //    }
        //}
        //if (isMine)
        //{
        //    // 해당 웨이 포인트 인덱스에 대한 아이템 프리팹이 있는지 확인
        //    if (waypointItemMapping.ContainsKey(wayPointIndex))
        //    {
        //    Debug.Log("tgdf");
        //        // 웨이 포인트 인덱스에 따른 아이템 인스턴스화
        //        GameObject itemToSpawn = waypointItemMapping[wayPointIndex];
        //        foreach (Transform spawnPoint in itemSpawnPoints)
        //        {
        //            Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);
        //        }
        //    }


        wayPointIndex = hitWaypoint.waypointIndex;
        //if (isMine)
        //{
        //    WaypointItemSpawnInfo spawnInfo = waypointItemSpawnInfos.Find(info => info.waypointIndex == wayPointIndex);
        //    if (spawnInfo != null)
        //    {
        //        Debug.Log("spawnInfo");
        //        foreach (Transform spawnPoint in spawnInfo.spawnPoints)
        //        {
        //            Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
        //        }
        //    }
        //}
        if (isMine)
        {
            WaypointItemSpawnInfo spawnInfo = waypointItemSpawnInfos.Find(info => info.waypointIndex == wayPointIndex);
            if (spawnInfo != null)
            {
                GameObject itemToSpawn = itemPrefabs[(int)spawnInfo.itemType]; // 아이템 타입에 따른 프리팹을 선택
                foreach (Transform spawnPoint in spawnInfo.spawnPoints)
                {
                    Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);
                }
            }
        }



        wayPointIndex = hitWaypoint.waypointIndex;
        prevWaypoint = hitWaypoint;
        nextWaypoint = hitWaypoint.NextPoint();
        checkPointCount++;



        if (nextWaypoint.waypointIndex == 0 && checkPointCount > 20)
        {
            checkPointCount = 0;
            lap++;
            if (lap == finalLap)
            {
                isFinish = true;
                if (isMine) KHHGameManager.instance.GameEnd();
            }
        }
    }

    public Vector3 GetReturnTrackPoint()
    {
        if (prevWaypoint != null && nextWaypoint != null)
            return Vector3.Lerp(prevWaypoint.transform.parent.position, nextWaypoint.transform.parent.position, wayPercent);
        else if (prevWaypoint != null)
            return prevWaypoint.transform.parent.position;
        else
            return Vector3.zero;
    }

}