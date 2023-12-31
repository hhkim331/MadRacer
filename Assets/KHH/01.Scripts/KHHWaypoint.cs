using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NextWaypoint
{
    public KHHWaypoint nextPoint;
    public float weight;
}

public class KHHWaypoint : MonoBehaviour
{
    public int waypointIndex;
    public NextWaypoint[] nextPoint;

    public Vector3 wayPosition;

    private void Start()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 10, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, 1<<LayerMask.NameToLayer("Default")))
        {
            wayPosition = hit.point;
        }
    }

    public KHHWaypoint NextPoint()
    {
        float totalWeight = 0;
        foreach (var item in nextPoint)
        {
            totalWeight += item.weight;
        }
        float randomValue = Random.Range(0, totalWeight);
        float weightSum = 0;
        foreach (var item in nextPoint)
        {
            weightSum += item.weight;
            if (randomValue <= weightSum)
            {
                return item.nextPoint;
            }
        }
        return null;
    }
}