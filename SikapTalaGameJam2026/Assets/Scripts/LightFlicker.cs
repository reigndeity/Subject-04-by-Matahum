using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Emission")]
    public Renderer rend;
    public Color emissionColor = Color.white;

    [Header("Light")]
    public Light targetLight;

    [Header("Emission Intensity")]
    public float normalEmissionIntensity = 2f;
    public float flickerEmissionMin = 0f;
    public float flickerEmissionMax = 0.5f;

    [Header("Light Intensity")]
    public float normalLightIntensity = 102.5f;
    public float flickerLightMin = 0f;
    public float flickerLightMax = 25f;

    [Header("Timing")]
    public float minInterval = 1f;
    public float maxInterval = 5f;

    [Header("Flicker Count")]
    public int minFlickers = 3;
    public int maxFlickers = 8;

    [Header("Flicker Speed")]
    public float flickerSpeedMin = 0.03f;
    public float flickerSpeedMax = 0.12f;

    public bool isFlickering = true;

    private Material mat;

    private Coroutine flickerCoroutine;

    [Header("Audio")]
    AudioSource src;

    void Start()
    {
        mat = rend.material;

        mat.EnableKeyword("_EMISSION");

        SetVisuals(
            normalEmissionIntensity,
            normalLightIntensity
        );

        flickerCoroutine = StartCoroutine(FlickerInterval());
    }

    IEnumerator FlickerInterval()
    {
        while (isFlickering)
        {
            // Wait before next flicker
            float waitTime = Random.Range(minInterval, maxInterval);

            yield return new WaitForSeconds(waitTime);

            // Random flicker amount
            int flickerCount =
                Random.Range(minFlickers, maxFlickers + 1);

            AudioManager.instance.PlayLightFlickerSFX(src);

            for (int i = 0; i < flickerCount; i++)
            {
                // Random dim values
                float emissionIntensity =
                    Random.Range(
                        flickerEmissionMin,
                        flickerEmissionMax
                    );

                float lightIntensity =
                    Random.Range(
                        flickerLightMin,
                        flickerLightMax
                    );

                SetVisuals(
                    emissionIntensity,
                    lightIntensity
                );

                float flickerTime =
                    Random.Range(
                        flickerSpeedMin,
                        flickerSpeedMax
                    );


                yield return new WaitForSeconds(flickerTime);

                // Return to normal
                SetVisuals(
                    normalEmissionIntensity,
                    normalLightIntensity
                );

                yield return new WaitForSeconds(flickerTime);
            }
        }
    }

    void SetVisuals(
        float emissionIntensity,
        float lightIntensity
    )
    {
        // Emission
        Color finalColor =
            emissionColor * emissionIntensity;

        mat.SetColor("_EmissionColor", finalColor);

        // Light
        if (targetLight != null)
        {
            targetLight.intensity = lightIntensity;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Monster>())
        {
            isFlickering = false;

            if (flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
            }

            SetVisuals(0, 0);
            AudioManager.instance.PlayLightGoneOutSFX(src);
        }
    }
}