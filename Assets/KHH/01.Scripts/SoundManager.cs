using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundData
{
    public string key;
    public AudioClip audioClip;
    public AudioMixerGroup audioMixerGroup;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    float bgmVolume = 1;
    public float BGMVolume
    {
        set
        {
            if (bgmCoroutine != null)
                StopCoroutine(bgmCoroutine);
            bgmCoroutine = StartCoroutine(BGMFade(bgmVolume, value, 1f));
            bgmVolume = value;
        }
    }
    Coroutine bgmCoroutine = null;

    float sfxVolume = 1;
    public float SFXVolume
    {
        set
        {
            if (sfxCoroutine != null)
                StopCoroutine(sfxCoroutine);
            sfxCoroutine = StartCoroutine(SFXFade(sfxVolume, value, 1f));
            sfxVolume = value;
        }
    }
    Coroutine sfxCoroutine = null;

    [SerializeField] AudioMixer audioMixer = null;

    [SerializeField] List<SoundData> loadingBGMSoundInfos = new List<SoundData>();
    [SerializeField] List<SoundData> loadingSFXSoundInfos = new List<SoundData>();

    Dictionary<string, SoundData> bgmContainer = new Dictionary<string, SoundData>();
    Dictionary<string, SoundData> sfxContainer = new Dictionary<string, SoundData>();

    GameObject bgmObj = null;   // ��׶��� ������Ʈ
    AudioSource bgmSrc = null;  // ��׶��� AudioSource ������Ʈ

    int sfxMaxCount = 10;
    int sfxCurCount = 0;
    List<GameObject> sfxObjList = new List<GameObject>(); //ArrayList m_sndObjList = new ArrayList();          // ȿ���� ������Ʈ
    AudioSource[] sfxSrcList;

    SoundData soundData = null;

    // Start is called before the first frame update

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        LoadSound();
        LoadChildGameObj();
    }

    void LoadSound()
    {
        //bgm
        for (int i = 0; i < loadingBGMSoundInfos.Count; i++)
        {
            bgmContainer.Add(loadingBGMSoundInfos[i].key, loadingBGMSoundInfos[i]);
        }

        //sfx
        for (int i = 0; i < loadingSFXSoundInfos.Count; i++)
        {
            sfxContainer.Add(loadingSFXSoundInfos[i].key, loadingSFXSoundInfos[i]);
        }

        sfxSrcList = new AudioSource[sfxMaxCount];
    }

    public void LoadChildGameObj()
    {
        //m_bgmObj == null �̸� PlayBGM()�ϰ� �Ǹ� �ٽ� �ε��ϰ� �ȴ�. 
        if (bgmObj == null)
        {
            bgmObj = new GameObject();
            bgmObj.transform.SetParent(this.transform);
            bgmObj.transform.position = Vector3.zero;
            bgmSrc = bgmObj.AddComponent<AudioSource>();
            bgmSrc.playOnAwake = false;
            bgmObj.name = "BGMObj";
        }

        for (int a_ii = 0; a_ii < sfxMaxCount; a_ii++)
        {
            // �ִ� 4������ ����ǰ� ���� ������(Androud: 4��, PC: ������)  
            if (sfxObjList.Count < sfxMaxCount)
            {
                GameObject newSoundOBJ = new GameObject();
                newSoundOBJ.transform.SetParent(this.transform);
                newSoundOBJ.transform.localPosition = Vector3.zero;
                AudioSource a_AudioSrc = newSoundOBJ.AddComponent<AudioSource>();
                a_AudioSrc.playOnAwake = false;
                a_AudioSrc.loop = false;
                newSoundOBJ.name = "SFXObj";

                sfxSrcList[sfxObjList.Count] = a_AudioSrc;
                sfxObjList.Add(newSoundOBJ);
            }
        }//for (int a_ii = 0; a_ii < m_EffSdCount; a_ii++)
    }

    #region BGM
    public void PlayBGM(string key)
    {
        soundData = bgmContainer[key];

        //Scene�� �Ѿ�� GameObject�� ��������, m_bgmObj == null �̸� 
        //PlayBGM()�ϰ� �Ǹ� �ٽ� �ε��ϰ� �ȴ�. 
        if (bgmObj == null)
        {
            bgmObj = new GameObject();
            bgmObj.transform.SetParent(this.transform);
            bgmObj.transform.position = Vector3.zero;
            bgmSrc = bgmObj.AddComponent<AudioSource>();
            bgmSrc.playOnAwake = false;
            bgmObj.name = "BGMObj";
        }

        if (soundData != null && bgmSrc != null)
        {
            if (bgmSrc.clip == soundData.audioClip)
                return;

            bgmSrc.clip = soundData.audioClip;
            bgmSrc.outputAudioMixerGroup = soundData.audioMixerGroup;
            //bgmSrc.volume = bgmVolume;
            bgmSrc.loop = true;
            bgmSrc.spatialBlend = 0f;
            bgmSrc.Play(0);
        }
    }

    IEnumerator BGMFade(float startValue, float endValue, float fadeTime)
    {
        float time = 0;
        while (time < fadeTime)
        {
            yield return null;
            audioMixer.SetFloat("BGM", Mathf.Lerp(startValue, endValue, time / fadeTime));
            time += Time.deltaTime;
        }
    }
    #endregion

    #region SFX
    //ȿ���� �÷��� �Լ�
    public void PlaySFX(string key, float delay = 0, bool bLoop = false)
    {
        if (sfxContainer.ContainsKey(key) == false)
            return;

        soundData = sfxContainer[key];

        // �ִ� 4������ ���
        if (sfxObjList.Count < sfxMaxCount)
        {
            GameObject newSoundOBJ = new GameObject();
            newSoundOBJ.transform.SetParent(this.transform);
            newSoundOBJ.transform.localPosition = Vector3.zero;
            AudioSource a_AudioSrc = newSoundOBJ.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            newSoundOBJ.name = "SFXObj";

            sfxSrcList[sfxObjList.Count] = a_AudioSrc;
            sfxObjList.Add(newSoundOBJ);
        }

        if (soundData != null && sfxSrcList[sfxCurCount] != null)
        {
            sfxSrcList[sfxCurCount].clip = soundData.audioClip;
            sfxSrcList[sfxCurCount].outputAudioMixerGroup = soundData.audioMixerGroup;
            sfxSrcList[sfxCurCount].spatialBlend = 0f;
            //sfxSrcList[sfxCurCount].volume = sfxVolume;
            sfxSrcList[sfxCurCount].loop = bLoop;
            sfxSrcList[sfxCurCount].PlayDelayed(delay);

            sfxCurCount++;
            if (sfxMaxCount <= sfxCurCount)
                sfxCurCount = 0;
        }
    }

    //������ ȿ���� �ѹ��� ȣ��
    public void PlaySFXOnce(string key, float delay = 0, bool bLoop = false)
    {
        if (sfxContainer.ContainsKey(key) == false)
            return;

        soundData = sfxContainer[key];

        foreach (var sfxSrc in sfxSrcList)
        {
            if (sfxSrc.clip == soundData.audioClip)
            {
                if (!sfxSrc.isPlaying)
                {
                    //sfxSrc.volume = sfxVolume;
                    sfxSrc.loop = bLoop;
                    sfxSrc.PlayDelayed(delay);
                }
                return;
            }
        }

        // �ִ� 10������ ���
        if (sfxObjList.Count < sfxMaxCount)
        {
            GameObject newSoundOBJ = new GameObject();
            newSoundOBJ.transform.SetParent(this.transform);
            newSoundOBJ.transform.localPosition = Vector3.zero;
            AudioSource a_AudioSrc = newSoundOBJ.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            newSoundOBJ.name = "SFXObj";

            sfxSrcList[sfxObjList.Count] = a_AudioSrc;
            sfxObjList.Add(newSoundOBJ);
        }

        if (soundData != null && sfxSrcList[sfxCurCount] != null)
        {
            sfxSrcList[sfxCurCount].clip = soundData.audioClip;
            sfxSrcList[sfxCurCount].outputAudioMixerGroup = soundData.audioMixerGroup;
            sfxSrcList[sfxCurCount].spatialBlend = 0f;
            //sfxSrcList[sfxCurCount].volume = sfxVolume;
            sfxSrcList[sfxCurCount].loop = bLoop;
            sfxSrcList[sfxCurCount].PlayDelayed(delay);

            sfxCurCount++;
            if (sfxMaxCount <= sfxCurCount)
                sfxCurCount = 0;
        }
    }

    public void PlaySFXFromObject(Vector3 soundPosition, string key, float delay = 0, bool bLoop = false)
    {
        if (sfxContainer.ContainsKey(key) == false)
            return;

        soundData = sfxContainer[key];

        // �ִ� 4������ ���
        if (sfxObjList.Count < sfxMaxCount)
        {
            GameObject newSoundOBJ = new GameObject();
            newSoundOBJ.transform.SetParent(this.transform);
            newSoundOBJ.transform.localPosition = Vector3.zero;
            AudioSource a_AudioSrc = newSoundOBJ.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            newSoundOBJ.name = "SFXObj";

            sfxSrcList[sfxObjList.Count] = a_AudioSrc;
            sfxObjList.Add(newSoundOBJ);
        }

        if (soundData != null && sfxSrcList[sfxCurCount] != null)
        {
            sfxSrcList[sfxCurCount].transform.position = soundPosition;

            sfxSrcList[sfxCurCount].clip = soundData.audioClip;
            sfxSrcList[sfxCurCount].outputAudioMixerGroup = soundData.audioMixerGroup;
            sfxSrcList[sfxCurCount].spatialBlend = 1f;
            //sfxSrcList[sfxCurCount].volume = sfxVolume;
            sfxSrcList[sfxCurCount].loop = bLoop;
            sfxSrcList[sfxCurCount].PlayDelayed(delay);

            sfxCurCount++;
            if (sfxMaxCount <= sfxCurCount)
                sfxCurCount = 0;
        }
    }

    IEnumerator SFXFade(float startValue, float endValue, float fadeTime)
    {
        float time = 0;
        while (time < fadeTime)
        {
            yield return null;
            audioMixer.SetFloat("SFX", Mathf.Lerp(startValue, endValue, time / fadeTime));
            time += Time.deltaTime;
        }
    }

    public void ClearAllSFX()
    {
        foreach (var src in sfxSrcList)
            src.Stop();
    }

    public void ChangeSFXPitch(float pitch)
    {
        foreach (var src in sfxSrcList)
        {
            if (src.clip == null)
                src.pitch = pitch;
            else if (src.clip.name == "super" || src.clip.name == "hot") src.pitch = 1;
            else src.pitch = pitch;
        }
    }
    #endregion
}