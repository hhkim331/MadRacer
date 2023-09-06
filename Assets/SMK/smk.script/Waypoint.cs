using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint nextPoint;
    public float radius = 1.0f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
        if (nextPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, nextPoint.transform.position);
        }
    }
}
