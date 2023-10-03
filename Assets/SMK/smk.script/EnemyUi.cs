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

    public Sprite[] IconImage;

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
        //if (EnemyEye.Instance.objectId == 0)
        //{

        //}
    }
}
