using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaler : MonoBehaviour
{
    [HideInInspector]
    public float scaleFactor = 1.0f; // ��ƼŬ ũ��

    /// <summary>
    /// �ڽŰ� ��� �ڽ� ������Ʈ�� Scaling Mode�� Hierarchy�� �����մϴ�.
    /// </summary>
    public void ParticleScalingModeChange()
    {
        ParticleSystem[] particleSystemList = GetComponentsInChildren<ParticleSystem>(true); // true�� �ϸ� ��Ȱ��ȭ �Ǿ��ִ� ������Ʈ�� ã���ش�.

        for (int i = 0; i < particleSystemList.Length; i++)
        {
            ParticleSystem.MainModule particle = particleSystemList[i].main;
            particle.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
    }

    /// <summary>
    /// �ڽŰ� ��� �ڽ� ������Ʈ�� Scale ���� scaleFactor������ �����մϴ�.
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
