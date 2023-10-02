using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class KHHPlayerHealth : KHHHealth
{
    KHHKart myKart;

    float hitTime = 0.5f;
    float hitDuration = 0.5f;
    public PostProcessProfile postProcessProfile;
    float intensity = 0.0f;
    float maxIntensity = 0.6f;

    Coroutine coHitEffect;
    public Animator hitAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        myKart = GetComponent<KHHKart>();
        postProcessProfile.GetSetting<Vignette>().intensity.value = 0;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Hit(float damage, KHHKartRank kart)
    {
        if (myKart.ShieldActive) return;
        if (kartRank.isFinish) return;
        base.Hit(damage, kart);
        SoundManager.instance.PlaySFX("Hit");
        if (health > 0)
        {
            hitTime = 0;
            if (coHitEffect != null)
                StopCoroutine(coHitEffect);
            coHitEffect = StartCoroutine(CoHitEffect());
            hitAnimator.transform.Rotate(Vector3.forward, Random.Range(0, 360));
            hitAnimator.SetTrigger("Hit");
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
