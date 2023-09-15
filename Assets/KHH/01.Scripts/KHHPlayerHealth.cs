using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class KHHPlayerHealth : KHHHealth
{
    //�ǰ� ȿ��
    float hitTime = 0.0f;
    public float hitDuration = 0.5f;
    public PostProcessProfile postProcessProfile;
    float intensity = 0.0f;
    public float maxIntensity = 0.3f;

    Coroutine coHitEffect;

    // Start is called before the first frame update
    void Start()
    {
        postProcessProfile.GetSetting<Vignette>().intensity.value = 0;
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        if (health > 0)
        {
            hitTime = 0;
            if (coHitEffect != null)
                StopCoroutine(coHitEffect);
            coHitEffect = StartCoroutine(CoHitEffect());
        }
    }

    public override void Die()
    {
        base.Die();
    }

    public override void Respawn()
    {
        base.Respawn();
        hitTime = 0;
        intensity = 0;
        postProcessProfile.GetSetting<Vignette>().intensity.value = 0;
    }

    IEnumerator CoHitEffect()
    {
        while (hitTime < hitDuration)
        {
            hitTime += Time.deltaTime;
            intensity += Time.deltaTime * maxIntensity / hitDuration;
            if (intensity > maxIntensity) intensity = maxIntensity;
            postProcessProfile.GetSetting<Vignette>().intensity.value = intensity;
            yield return null;
        }

        while (hitTime > 0)
        {
            hitTime -= Time.deltaTime;
            intensity -= Time.deltaTime * maxIntensity / hitDuration;
            if (intensity < 0) intensity = 0;
            postProcessProfile.GetSetting<Vignette>().intensity.value = intensity;
            yield return null;
        }
    }
}
