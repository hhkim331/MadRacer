using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaler : MonoBehaviour
{
    [HideInInspector]
    public float scaleFactor = 1.0f; // 파티클 크기

    /// <summary>
    /// 자신과 모든 자식 오브젝트의 Scaling Mode를 Hierarchy로 변경합니다.
    /// </summary>
    public void ParticleScalingModeChange()
    {
        ParticleSystem[] particleSystemList = GetComponentsInChildren<ParticleSystem>(true); // true를 하면 비활성화 되어있는 오브젝트도 찾아준다.

        for (int i = 0; i < particleSystemList.Length; i++)
        {
            ParticleSystem.MainModule particle = particleSystemList[i].main;
            particle.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
    }

    /// <summary>
    /// 자신과 모든 자식 오브젝트의 Scale 값을 scaleFactor값으로 변경합니다.
    /// </summary>
    public void ParticleScaleChange()
    {
        ParticleSystem[] particleSystemList = GetComponentsInChildren<ParticleSystem>(true);

        for (int i = 0; i < particleSystemList.Length; i++)
        {
            particleSystemList[i].gameObject.transform.localScale = Vector3.one * scaleFactor;
        }
    }
}
