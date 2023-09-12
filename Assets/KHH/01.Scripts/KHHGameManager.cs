using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHHGameManager : MonoBehaviour
{
    public KHHKartRank[] kartRanks;

    // Update is called once per frame
    void Update()
    {
        //모든 카트 순위 계산
        foreach (var item in kartRanks)
        {
            item.rank = 1;
            foreach (var other in kartRanks)
            {
                if (item == other) continue;
                if (item.lap > other.lap) continue;
                if (item.lap == other.lap && item.wayPointIndex > other.wayPointIndex) continue;
                if (item.lap == other.lap && item.wayPointIndex == other.wayPointIndex && item.wayPercent > other.wayPercent) continue;
                item.rank++;
            }
        }
    }
}
