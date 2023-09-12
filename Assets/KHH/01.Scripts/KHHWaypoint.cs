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