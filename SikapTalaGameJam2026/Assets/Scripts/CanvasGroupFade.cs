using System.Collections;
using UnityEngine;

public class CanvasGroupFade : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    Coroutine fadeRoutine;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        UpdateRaycastState();
    }

    public void Fade(float start, float end, float duration)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeRoutine(start, end, duration));
    }

    IEnumerator FadeRoutine(float start, float end, float duration)
    {
        canvasGroup.alpha = start;
        UpdateRaycastState();

        if (duration <= 0f)
        {
            canvasGroup.alpha = end;
            UpdateRaycastState();
            yield break;
        }

        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, time / duration);
            UpdateRaycastState();
            yield return null;
        }

        canvasGroup.alpha = end;
        UpdateRaycastState();
    }

    void UpdateRaycastState()
    {
        bool block = canvasGroup.alpha > 0f;
        canvasGroup.blocksRaycasts = block;
        canvasGroup.interactable = block;
    }
}