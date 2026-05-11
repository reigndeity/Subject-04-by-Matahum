using System.Collections;
using Febucci.UI;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue UI")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;

    [Header("Typing")]
    [SerializeField] TypewriterByCharacter typewriter;

    [Header("Spacebar Indicator")]
    public CanvasGroup spacebarIndicator;

    [Header("Input")]
    public float spacebarCooldown = 0.2f;

    string[] dialogueLines;
    int currentLine;

    float nextAllowedSpacebarTime;
    Coroutine indicatorRoutine;

    void Awake()
    {
        instance = this;

        if (spacebarIndicator != null)
        {
            spacebarIndicator.alpha = 0f;
            spacebarIndicator.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!IsInDialogue())
            return;

        bool finishedTyping = typewriter == null || typewriter.TextAnimator.allLettersShown;

        if (finishedTyping)
            ShowSpacebarIndicator();
        else
            HideSpacebarIndicator();

        if (!finishedTyping)
            return;

        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (Time.time < nextAllowedSpacebarTime)
            return;

        nextAllowedSpacebarTime = Time.time + spacebarCooldown;
        NextLine();
    }

    public void BeginDialogue(string[] dialogue)
    {
        dialogueLines = dialogue;
        currentLine = 0;
        nextAllowedSpacebarTime = Time.time + spacebarCooldown;

        dialoguePanel.SetActive(true);
        Player.instance.inputLocked = true;

        SetCurrentLine();
    }

    public IEnumerator PlayDialogue(string[] dialogue)
    {
        BeginDialogue(dialogue);

        while (IsInDialogue())
            yield return null;
    }

    public void NextLine()
    {
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        SetCurrentLine();
    }

    void SetCurrentLine()
    {
        HideSpacebarIndicator();

        if (typewriter != null)
            typewriter.ShowText(dialogueLines[currentLine]);
        else
            dialogueText.text = dialogueLines[currentLine];
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        StartCoroutine(UnlockPlayerNextFrame());

        HideSpacebarIndicator();
    }
    IEnumerator UnlockPlayerNextFrame()
    {
        yield return null;
        Player.instance.inputLocked = false;
    }

    public bool IsInDialogue()
    {
        return dialoguePanel.activeSelf;
    }

    void ShowSpacebarIndicator()
    {
        if (spacebarIndicator == null)
            return;

        if (spacebarIndicator.gameObject.activeSelf)
            return;

        spacebarIndicator.gameObject.SetActive(true);

        if (indicatorRoutine != null)
            StopCoroutine(indicatorRoutine);

        indicatorRoutine = StartCoroutine(FadeSpacebarIndicator());
    }

    void HideSpacebarIndicator()
    {
        if (spacebarIndicator == null)
            return;

        if (indicatorRoutine != null)
        {
            StopCoroutine(indicatorRoutine);
            indicatorRoutine = null;
        }

        spacebarIndicator.alpha = 0f;
        spacebarIndicator.gameObject.SetActive(false);
    }

    IEnumerator FadeSpacebarIndicator()
    {
        while (true)
        {
            yield return FadeAlpha(0f, 0.8f, 0.6f);
            yield return FadeAlpha(0.8f, 0f, 0.6f);
        }
    }

    IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            spacebarIndicator.alpha = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }

        spacebarIndicator.alpha = to;
    }
}