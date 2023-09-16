using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHHPlayerUI : MonoBehaviour
{
    KHHKartRank myKartRank;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI lapText;

    private void Start()
    {
        myKartRank = GetComponent<KHHKartRank>();
    }

    private void Update()
    {
        rankText.text = string.Format("¼øÀ§:{0}/5", myKartRank.rank);
        lapText.text = string.Format("·¦:{0}/2", myKartRank.lap);
    }
}
