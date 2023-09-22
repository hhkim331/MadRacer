using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHHPlayerUI : MonoBehaviour
{
    const string playerKillEnemy = "당신이 {0}(을)를 처치했습니다!!!";
    const string enemyKillPlayer = "{0}이 당신을 죽였습니다!!!";
    const string enemyKillEnemy = "{0}이 {1}(을)를 처치했습니다!!!";
    const string subWeaponReady = "당신의 특별한 무기가 준비 되었습니다!";
    const string noEnergyBoost = "<color=#4285f4>에너지</color>가 부족해서 부스트를 사용할 수 없습니다";
    const string noEnergyShield = "<color=#4285f4>에너지</color>가 부족해서 쉴드를 사용할 수 없습니다";
    const string lapTime = "시간: {0,0:00}:{1,0:00}:{2,0:000}";
    const string noBullet = "탄약이 없습니다";

    public KHHKartRank myKartRank;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI lapText;

    //info
    float infoTime = 0;
    float infoTimeMax = 1;
    public TextMeshProUGUI infoText;

    private void Update()
    {
        rankText.text = string.Format("순위:{0}/5", myKartRank.rank);
        lapText.text = string.Format("랩:{0}/2", myKartRank.lap);

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
