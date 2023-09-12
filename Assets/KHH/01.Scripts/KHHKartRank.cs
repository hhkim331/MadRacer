using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHKartRank : MonoBehaviour
{
    public int rank = 0;
    public int lap = 0;
    public int wayPointIndex = 0;
    public float wayPercent = 0;

    int checkPointCount = 0;

    public KHHWaypoint prevWaypoint;
    public KHHWaypoint nextWaypoint;

    // Update is called once per frame
    void Update()
    {
        wayPercent = Vector3.Distance(prevWaypoint.transform.parent.position, transform.position) /
            (Vector3.Distance(prevWaypoint.transform.parent.position, transform.position) +
            Vector3.Distance(nextWaypoint.transform.parent.position, transform.position));
    }

    private void OnTriggerEnter(Collider other)
    {
        KHHWaypoint hitWaypoint = other.GetComponent<KHHWaypoint>();
        if (hitWaypoint == null) return;
        if (wayPointIndex == hitWaypoint.waypointIndex) return;
        wayPointIndex = hitWaypoint.waypointIndex;
        prevWaypoint = hitWaypoint;
        nextWaypoint = hitWaypoint.NextPoint();
        checkPointCount++;

        if (nextWaypoint.waypointIndex == 0 && checkPointCount > 20)
        {
            checkPointCount = 0;
            lap++;
        }
    }
}
