using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public static EnemySound Instance;
    public void Awake()
    {
        if (EnemySound.Instance == null)
        {
            Instance = this;
        }
    }
    [System.Serializable]
    public struct EnemyBgm
    {
        public string name;
        public AudioClip sound;

    }

    public EnemyBgm[] SoundList;
    AudioSource audioSource;

    #region 사운드 상태전환
    public enum Soundstate
    {
        Move,
        Attack,
        Booster,
        Drift,
        Hit,
        Die,
        Idle,
    }
    #endregion

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public Soundstate state;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case Soundstate.Move: Move(); break;
            case Soundstate.Attack: Attack(); break;
            case Soundstate.Booster: Booster(); break;
            case Soundstate.Drift: Drift(); break;
            case Soundstate.Hit: Hit(); break;
            case Soundstate.Die: Die(); break;
            case Soundstate.Idle: Idle(); break;
        }
    }
     

    public void Idle()
    {
        //움직이지 않을때 잠깐 소리 나오는 엔진 켜는 소리
        audioSource.PlayOneShot(SoundList[6].sound, 0.5f);
    }

    public void Die()
    {
        audioSource.PlayOneShot(SoundList[5].sound, 0.5f);

    }

    public void Hit()
    {
        audioSource.PlayOneShot(SoundList[4].sound, 0.5f);
    }

    public void Drift()
    {
        audioSource.Pause();

        audioSource.PlayOneShot(SoundList[3].sound, 0.5f);
    }

    public void Booster()
    {
        audioSource.Pause();

        audioSource.PlayOneShot(SoundList[2].sound, 0.5f);
    }

    public void Attack()
    {
        audioSource.Pause();

        audioSource.PlayOneShot(SoundList[1].sound, 0.5f);
    }

    public void Move()
    {
        audioSource.Pause();

        audioSource.PlayOneShot(SoundList[0].sound, 0.5f);

    }

    #region 남긴거
    //public void PlaySound(string name)
    //{
    //    //현재 플레이하는 사운드를 바꿈.
    //    //if (NowBGMname.Equals(name)) return;

    //    //for (int i = 0; i < SoundList.Length; ++i)
    //    //    if (SoundList[i].name.Equals(name))
    //    //    {
    //    //        audioSource.clip = SoundList[i].sound;
    //    //        audioSource.Play();
    //    //        NowBGMname = name;
    //    //    }
    //} 
    #endregion
}
