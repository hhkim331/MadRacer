using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class KHHHealth : MonoBehaviour
{
    int health = 10;
    public int maxHealth = 10;
    public Image healthBar;

    //피격 효과
    float hitTime = 0.0f;
    public float hitDuration = 0.5f;
    public PostProcessProfile postProcessProfile;
    float intensity = 0.0f;
    public float maxIntensity = 0.3f;

    //사망 효과
    public GameObject explosionPrefab;

    Coroutine coHitEffect;

    // Start is called before the first frame update
    void Start()
    {
        postProcessProfile.GetSetting<Vignette>().intensity.value = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hit()
    {
        if (health > 0)
        {
            health--;
            healthBar.fillAmount = (float)health / maxHealth;
            hitTime = 0;
            if (coHitEffect != null)
                StopCoroutine(coHitEffect);
            coHitEffect = StartCoroutine(CoHitEffect());
        }
    }

    void Die()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position + Vector3.up;
        Destroy(explosion, 4);
    }

    public void Respawn()
    {
        health = maxHealth;
        healthBar.fillAmount = 1;
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
