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

    #region ���� ������ȯ
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
        //�������� ������ ��� �Ҹ� ������ ���� �Ѵ� �Ҹ�
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
