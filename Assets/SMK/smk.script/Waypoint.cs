using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint nextPoint;
    public float radius = 1.0f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        if (nextPoint != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, nextPoint.transform.position);
        }
    }
}
