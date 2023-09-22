using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class KHHGameManager : MonoBehaviour
{
    public static KHHGameManager instance;

    KHHPlayerUI playerUI;
    public KHHPlayerUI PlayerUI { get { return playerUI; } }

    public float time = 0;
    //start
    public bool isStart = false;
    public bool isEnd = false;
    public TextMeshProUGUI startText;

    public GameObject[] kartObjs;

    public GameObject rankUIObj;
    KHHKartRank[] kartRanks;

    public Transform vrCam;
    public PostProcessProfile postProcessProfile;

    private void Awake()
    {
        instance = this;
        playerUI = GetComponent<KHHPlayerUI>();
    }

    private void Start()
    {
        SoundManager.instance.PlayBGM("Stage");

        kartRanks = new KHHKartRank[kartObjs.Length];
        for (int i = 0; i < kartObjs.Length; i++)
            kartRanks[i] = kartObjs[i].GetComponent<KHHKartRank>();

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        startText.text = "3";
        SoundManager.instance.PlaySFX("StartCount");
        yield return new WaitForSeconds(1f);
        startText.text = "2";
        SoundManager.instance.PlaySFX("StartCount");
        yield return new WaitForSeconds(1f);
        startText.text = "1";
        SoundManager.instance.PlaySFX("StartCount");
        yield return new WaitForSeconds(1f);
        startText.text = "GO!";
        SoundManager.instance.PlaySFX("StartCountEnd");
        yield return new WaitForSeconds(0.1f);
        isStart = true;
        time = 0;
        yield return new WaitForSeconds(0.9f);
        startText.gameObject.SetActive(false);
        rankUIObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart) return;

        time += Time.deltaTime;
        //모든 카트 순위 계산
        foreach (var kartRank in kartRanks)
        {
            if (kartRank.isFinish) continue;
            kartRank.rank = 1;
            foreach (var other in kartRanks)
            {
                if (other.isFinish)
                {
                    kartRank.rank++;
                    continue;
                }
                if (kartRank == other) continue;
                if (kartRank.lap > other.lap) continue;
                if (kartRank.lap == other.lap && kartRank.nextWaypoint.waypointIndex > other.nextWaypoint.waypointIndex) continue;
                if (kartRank.lap == other.lap && kartRank.nextWaypoint.waypointIndex == other.nextWaypoint.waypointIndex && kartRank.wayPercent > other.wayPercent) continue;
                kartRank.rank++;
            }
        }
    }

    public void GameEnd()
    {
        StopEngineSound();
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
        isEnd = true;
    }

    void StopEngineSound()
    {
        SoundManager.instance.StopEngine("EngineIdle");
        SoundManager.instance.StopEngine("EngineAccel");
        SoundManager.instance.StopEngine("EngineDrift");
        SoundManager.instance.StopEngine("EngineBrake");
    }
}
