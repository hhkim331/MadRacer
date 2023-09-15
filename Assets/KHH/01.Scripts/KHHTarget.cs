using UnityEngine;

public class KHHTarget : MonoBehaviour
{
    public enum HitType
    {
        None,
        Metal,
        Sand,
        Stone
    }

    public HitType hitType = HitType.None;
}
