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

    public Image img_render;
    public Sprite[] IconImage;

    void Start()
    {
        kHHKartRank = GetComponent<KHHKartRank>();
        
        UICanvace.SetActive(false);
        NameText.text = kHHKartRank.name;
        if (EnemyEye.Instance.objectId == 0)
        {
            img_render.sprite = IconImage[0];
        }
        if (EnemyEye.Instance.objectId == 1)
        {
            img_render.sprite = IconImage[1];
        }
        if (EnemyEye.Instance.objectId == 2)
        {
            img_render.sprite = IconImage[2];
        }
        if (EnemyEye.Instance.objectId == 3)
        {
            img_render.sprite = IconImage[3];
        }

    }

    void Update()
    {
        //바라보게 하기.
        UICanvace.transform.LookAt(Camera.main.transform);
    }
}
