using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMove : MonoBehaviour
{
    //범위안에 들어오면 공격.
    //범위안에 있으면 아이템 사용

    //콜라이더에 부딪히면
    //피격


    float attackMax = 3f;
    //"kart"태그 달린 모두를 공격
    void Start()
    {
        
    }

    void Update()
    {
        //적과 다른 오브젝트(적, 플레이어)와의 거리가 attackMax보다 작으면,
        //총알공격
        //아이템을 가지고있을 경우 사용



        //다른 오브젝트와 맞닿았을 때, 피격 판정으로 hp 깍임.
        //총알, 범위 공격에 닿았을 경우, 피격 판정.
        //아이템과 닿았을 경우, 저장.
    }
}
