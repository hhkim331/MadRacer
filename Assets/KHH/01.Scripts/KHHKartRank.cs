using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;
using static KHHWaypoint;

[System.Serializable]
public class WaypointItemSpawnInfo
{
    public int waypointIndex; // ��������Ʈ �ε���
    public ItemType itemType; // ������ �������� ����
    public Transform[] spawnPoints; // �ش� �ε������� �������� ������ ��ġ
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

    public Transform[] itemSpawnPoints;

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
        //    // ��������Ʈ�� ���� �� ������ ������ �ν��Ͻ�ȭ
        //    foreach (Transform spawnPoint in itemSpawnPoints)
        //    {
        //        Debug.Log("cani");
        //        Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
        //    }
        //}
        //if (isMine)
        //{
        //    // �ش� ���� ����Ʈ �ε����� ���� ������ �������� �ִ��� Ȯ��
        //    if (waypointItemMapping.ContainsKey(wayPointIndex))
        //    {
        //    Debug.Log("tgdf");
        //        // ���� ����Ʈ �ε����� ���� ������ �ν��Ͻ�ȭ
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
                GameObject itemToSpawn = itemPrefabs[(int)spawnInfo.itemType]; // ������ Ÿ�Կ� ���� �������� ����
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