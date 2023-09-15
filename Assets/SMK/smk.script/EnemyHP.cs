using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHP : MonoBehaviour
{
    public int hp = 0;
    public int MaxHP = 100;
    public Slider sliderHP;


    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            sliderHP.value = hp;
        }
    }
    void Start()
    {
        sliderHP.maxValue = MaxHP;
        HP = MaxHP;
    }
}
