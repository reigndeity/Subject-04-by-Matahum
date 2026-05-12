using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TutorialCanvasGroup : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeInDuration = 0.25f;
    public float showDuration = 2f;
    public float fadeOutDuration = 1f;
    public KeyCode key = KeyCode.W;

    [Header("Tutorial Type")]
    public bool useKeyboard = true;
    public bool useMouse = false;

    public UnityEvent onFadeInComplete;
    public UnityEvent onFadeOutComplete;

    Coroutine currentRoutine;
    bool canFadeOut;
    bool conditionMet;

    void OnEnable()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        canFadeOut = false;
        conditionMet = false;

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        currentRoutine = StartCoroutine(Fade(0f, 1f, fadeInDuration, true));
    }

    void Update()
    {
        if (!canFadeOut || conditionMet)
            return;

        bool shouldFadeOut = false;

        if (useKeyboard)
        {
            shouldFadeOut =
                Input.GetKeyDown(key) ||
                Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.UpArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.RightArrow);
        }

        if (!shouldFadeOut && useMouse)
        {
            shouldFadeOut =
                Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f ||
                Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f;
        }

        if (!shouldFadeOut)
            return;

        conditionMet = true;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowThenFadeOut());
    }

    IEnumerator ShowThenFadeOut()
    {
        yield return new WaitForSeconds(showDuration);
        yield return Fade(canvasGroup.alpha, 0f, fadeOutDuration, false);
    }

    IEnumerator Fade(float start, float end, float duration, bool blockRaycastsOnEnd)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }

        canvasGroup.alpha = end;
        canvasGroup.blocksRaycasts = blockRaycastsOnEnd;

        if (end == 1f)
        {
            canFadeOut = true;
            onFadeInComplete?.Invoke();
        }
        else if (end == 0f)
        {
            onFadeOutComplete?.Invoke();
        }
    }
}