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
    public Vector3 WayForward { get { return (nextWaypoint.transform.parent.position - prevWaypoint.transform.parent.position).normalized; } }

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
