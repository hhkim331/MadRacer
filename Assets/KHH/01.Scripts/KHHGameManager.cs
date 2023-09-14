using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class KHHGameManager : MonoBehaviour
{
    public static KHHGameManager instance;

    //start
    public bool isStart = false;
    public TextMeshProUGUI startText;

    public GameObject[] kartObjs;

    public GameObject rankUIObj;
    KHHKartRank[] kartRanks;

    public Transform vrCam;
    public PostProcessProfile postProcessProfile;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        kartRanks = new KHHKartRank[kartObjs.Length];
        for (int i = 0; i < kartObjs.Length; i++)
            kartRanks[i] = kartObjs[i].GetComponent<KHHKartRank>();

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        startText.text = "3";
        yield return new WaitForSeconds(1f);
        startText.text = "2";
        yield return new WaitForSeconds(1f);
        startText.text = "1";
        yield return new WaitForSeconds(1f);
        startText.text = "GO!";
        yield return new WaitForSeconds(0.1f);
        isStart = true;
        yield return new WaitForSeconds(0.9f);
        startText.gameObject.SetActive(false);
        rankUIObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart) return;

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

    public void GameEnd()
    {
        StartCoroutine(CoGameEnd());
    }

    IEnumerator CoGameEnd()
    {
        ColorGrading colorGrading = postProcessProfile.GetSetting<ColorGrading>();
        float value = 1f;
        float delay = 0.5f;
        while (value > 0)
        {
            value -= Time.deltaTime / delay;
            if (value < 0) value = 0;
            Color color = new Color(value, value, value, 1);
            colorGrading.colorFilter.value = color;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);
        vrCam.transform.parent = null;
        vrCam.transform.position = new Vector3(0, 5000, 0);
        rankUIObj.transform.localPosition = Vector3.zero;
        rankUIObj.transform.localScale = Vector3.one * 2f;

        while (value < 1)
        {
            value += Time.deltaTime / delay;
            if (value > 1) value = 1;
            Color color = new Color(value, value, value, 1);
            colorGrading.colorFilter.value = color;
            yield return null;
        }
    }
}
