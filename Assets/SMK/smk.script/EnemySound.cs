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
    public AudioSource[] audioSource;
    bool boosterChange;
    #region ���� ������ȯ
    public enum Soundstate
    {
        Move,
        Booster,
        Die,
        Attack,
        Idle,
    }
    #endregion

    void Start()
    {
        boosterChange = false;
        //audioSource = GetComponent<AudioSource>();
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
        }
    }


    public void Booster()
    {
        boosterChange = true;
        audioSource[0].Pause();
        audioSource[0].PlayOneShot(SoundList[2].sound, 0.5f);
        boosterChange = false;
    }

    public void Attack()
    {
        audioSource[1].PlayOneShot(SoundList[1].sound, 0.5f);
    }

    public void Move()
    {
        if (boosterChange == false)
        {
            audioSource[0].Pause();
        }
        audioSource[0].PlayOneShot(SoundList[0].sound, 0.5f);
    }

    #region �����
    //public void PlaySound(string name)
    //{
    //    //���� �÷����ϴ� ���带 �ٲ�.
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