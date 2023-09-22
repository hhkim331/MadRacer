using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    [System.Serializable]
    public class DropInfo
    {
        public int waypointIndex;
        public Transform dropPoint;
        public Item.ItemType itemType;
    }
    public List<DropInfo> dropInfos = new List<DropInfo>();

    public Transform GetDropPointForWaypoint(int waypointIndex, out Item.ItemType itemType)
    {
        foreach (var info in dropInfos)
        {
            if (info.waypointIndex == waypointIndex)
            {
                itemType = info.itemType;
                return info.dropPoint;
            }
        }
        itemType = default;
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
