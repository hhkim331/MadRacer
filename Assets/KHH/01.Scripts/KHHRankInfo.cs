using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHHRankInfo : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI timeText;

    public void Set(int rank)
    {
        rankText.text = rank.ToString();
    }

    public void SetRankText(bool finish, string name, float time)
    {
        nameText.text = name;

        if (finish)
        {
            int min = (int)(time / 60);
            int sec = (int)(time % 60);
            int milli = (int)((time - (int)time) * 1000);
            timeText.text = string.Format("{0,0:00}:{1,0:00}:{2,0:000}", min, sec, milli);
        }
        else
        {
            timeText.text = "";
        }
    }
}
