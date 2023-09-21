using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public string name;
    public bool isFinish = false;
    public int rank = 0;
    public int lap = 0;
    public int finalLap = 2;
    public int finalRank = 0;
    public float time = 0;

    public int waypointIndex = 0;
    public float wayPercent = 0;
    public GameObject itemPrefab;
    bool isMine = false;
    public bool IsMine { get { return isMine; } set { isMine = value; } }
    public List<WaypointItemSpawnInfo> waypointItemSpawnInfos = new List<WaypointItemSpawnInfo>();

    int checkPointCount = 0;

    public KHHWaypoint prevWaypoint;
    public KHHWaypoint nextWaypoint;
    public Vector3 WayForward { get { return (nextWaypoint.transform.parent.position - prevWaypoint.transform.parent.position).normalized; } }

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
        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();
        if (hitWaypoint == null) return;
        if (waypointIndex == hitWaypoint.waypointIndex) return;

        waypointIndex = hitWaypoint.waypointIndex;

        if (isMine)
        {
            WaypointItemSpawnInfo spawnInfo = waypointItemSpawnInfos.Find(info => info.waypointIndex == waypointIndex);
            if (spawnInfo != null)
            {
                GameObject itemToSpawn = itemPrefabs[(int)spawnInfo.itemType]; // 아이템 타입에 따른 프리팹을 선택
                foreach (Transform spawnPoint in spawnInfo.spawnPoints)
                {
                    Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);
                }
            }
        }

        waypointIndex = hitWaypoint.waypointIndex;
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
                finalRank = rank;
                time = KHHGameManager.instance.time;
                if (isMine) KHHGameManager.instance.GameEnd();
            }
            else
            {
                KHHGameManager.instance.PlayerUI.LapTime(time);
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