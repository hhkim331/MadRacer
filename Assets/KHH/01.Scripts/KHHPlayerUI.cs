using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHHPlayerUI : MonoBehaviour
{
    const string playerKillEnemy = "����� {0}(��)�� óġ�߽��ϴ�!!!";
    const string enemyKillPlayer = "{0}�� ����� �׿����ϴ�!!!";
    const string enemyKillEnemy = "{0}�� {1}(��)�� óġ�߽��ϴ�!!!";
    const string subWeaponGrab = "����ϱ� ���� ����ּ���";
    const string subWeaponReady = "����� Ư���� ���Ⱑ �غ� �Ǿ����ϴ�!";
    const string noEnergyBoost = "<color=#4285f4>������</color>�� �����ؼ� �ν�Ʈ�� ����� �� �����ϴ�";
    const string noEnergyShield = "<color=#4285f4>������</color>�� �����ؼ� ���带 ����� �� �����ϴ�";
    const string lapTime = "�ð�: {0,0:00}:{1,0:00}:{2,0:000}";
    const string noBullet = "ź���� �����ϴ�";

    public KHHKartRank myKartRank;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI lapText;

    //info
    float infoTime = 0;
    float infoTimeMax = 1;
    public TextMeshProUGUI infoText;

    private void Update()
    {
        rankText.text = string.Format("����:{0}/5", myKartRank.rank);
        lapText.text = string.Format("��:{0}/2", myKartRank.lap);

        if (infoTime > 0)
        {
            infoTime -= Time.deltaTime;
            if (infoTime <= 0)
            {
                infoTime = 0;
                infoText.gameObject.SetActive(false);
            }
        }
    }

    public void PlayerKillEnemy(string enemyName)
    {
        infoTime = infoTimeMax;
        infoText.gameObject.SetActive(true);
        infoText.text = string.Format(playerKillEnemy, enemyName);
    }

    public void EnemyKillPlayer(string enemyName)
    {
        infoTime = infoTimeMax;
        infoText.gameObject.SetActive(true);
        infoText.text = string.Format(enemyKillPlayer, enemyName);
    }

    public void EnemyKillEnemy(string enemyName, string enemyName2)
    {
        infoTime = infoTimeMax;
        infoText.gameObject.SetActive(true);
        infoText.text = string.Format(enemyKillEnemy, enemyName, enemyName2);
    }

    public void SubWeaponGrab()
    {
        infoTime = infoTimeMax;
        infoText.gameObject.SetActive(true);
        infoText.text = subWeaponGrab;
    }

    public void SubWeaponReady()
    {
        infoTime = infoTimeMax;
        infoText.gameObject.SetActive(true);
        infoText.text = subWeaponReady;
    }

    public void NoEnergyBoost()
    {
        infoTime = infoTimeMax;
        infoText.gameObject.SetActive(true);
        infoText.text = noEnergyBoost;
    }

    public void NoEnergyShield()
    {
        infoTime = infoTimeMax;
        infoText.gameObject.SetActive(true);
        infoText.text = noEnergyShield;
    }

    public void LapTime(float time)
    {
        infoTime = infoTimeMax;
        infoText.gameObject.SetActive(true);
        int min = (int)(time / 60);
        int sec = (int)(time % 60);
        int milli = (int)((time - (int)time) * 1000);
        infoText.text = string.Format(lapTime, min, sec, milli);
    }

    public void NoBullet()
    {
        infoTime = infoTimeMax;
        infoText.gameObject.SetActive(true);
        infoText.text = noBullet;
    }
}
