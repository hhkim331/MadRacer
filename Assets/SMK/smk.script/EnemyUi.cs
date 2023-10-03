using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUi : MonoBehaviour
{
    public GameObject UICanvace;
    public TMP_Text NameText;

    KHHKartRank kHHKartRank;

    void Start()
    {
        kHHKartRank = GetComponent<KHHKartRank>();

        UICanvace.SetActive(false);
        NameText.text = kHHKartRank.name;
    }

    void Update()
    {
        //바라보게 하기.
        UICanvace.transform.LookAt(Camera.main.transform);
    }
}
