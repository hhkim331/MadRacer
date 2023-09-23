using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUi : MonoBehaviour
{
    public GameObject UICanvace;
    public Text NameText;

    KHHKartRank kHHKartRank;

    void Start()
    {
        kHHKartRank = GetComponent<KHHKartRank>();

        UICanvace.SetActive(false);
        NameText.text = kHHKartRank.name;
    }

    void Update()
    {
        //�ٶ󺸰� �ϱ�.
        UICanvace.transform.LookAt(Camera.main.transform);
    }
}
