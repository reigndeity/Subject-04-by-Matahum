using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Cutscenes")]
    public PlayableDirector playableDirector;
    public PlayableAsset cutscene1;

    [Header("Tutorials")]
    public GameObject movementTutorial;
    public GameObject jumpTutorial;
    public GameObject mouseTutorial;

    void Awake()
    {
        instance = this;
        playableDirector.playableAsset = null;
    }

    void Start()
    {
        //StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        Player.instance.inputLocked = true;
        Player.instance.StopMovement();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playableDirector.Play(cutscene1);

        yield return new WaitForSeconds((float)cutscene1.duration);

        yield return DialogueManager.instance.PlayDialogue(new string[]
        {
            "Huh..?",
            "Where am I?",
            "I don't remember how I got here."
        });

        movementTutorial.SetActive(true);
    }

    public void CCTVTutorial(int current)
    {
        playableDirector.Pause();
        StartCoroutine(DoingCCTVTutorial(current));
    }

    IEnumerator DoingCCTVTutorial(int current)
    {
        switch (current)
        {
            case 0:
                yield return DialogueManager.instance.PlayDialogue(new string[]
                {
                    "Interesting...",
                    "It seems I can control the CCTV using the <color=yellow>WASD</color> keys"
                });
                playableDirector.Resume();
                break;

            case 1:
                yield return DialogueManager.instance.PlayDialogue(new string[]
                {
                    "Huh?",
                    "What the hell is that?!"
                });
                playableDirector.Resume();
                break;

            case 2:
                yield return DialogueManager.instance.PlayDialogue(new string[]
                {
                    "What the?!",
                    "I can't see it!",
                    "AM I GOING INSANE?!"
                });
                playableDirector.Resume();
                break;

            case 3:
                yield return DialogueManager.instance.PlayDialogue(new string[]
                {
                    "IT'S COMING TOWARDS ME!",
                    "I NEED TO RUN!"
                });
                playableDirector.Resume();
                break;
        }
    }

    public bool IsCutscenePlaying()
    {
        if (playableDirector == null || playableDirector.playableAsset == null)
            return false;

        return playableDirector.time > 0 &&
            playableDirector.time < playableDirector.duration;
    }

    public void UnlockPlayer()
    {
        Player.instance.inputLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void DisableTutorial()
    {
        movementTutorial.SetActive(false);
        jumpTutorial.SetActive(false);
        mouseTutorial.SetActive(false);
    }
}