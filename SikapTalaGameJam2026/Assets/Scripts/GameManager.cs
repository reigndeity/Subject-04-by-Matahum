using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CanvasGroupFade canvasGroupFade;
    public int checkpointIndex;

    public Transform player;
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Death();
        }
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
                    "I NEED TO GET OUT OF HERE!"
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

    public void Save(int index)
    {
        checkpointIndex = index;
        PlayerPrefs.SetInt("CheckpointIndex", checkpointIndex);

        Vector3 pos = player.position;

        PlayerPrefs.SetFloat("CheckpointX", pos.x);
        PlayerPrefs.SetFloat("CheckpointY", pos.y);
        PlayerPrefs.SetFloat("CheckpointZ", pos.z);

        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey("CheckpointX"))
            return;

        checkpointIndex = PlayerPrefs.GetInt("CheckpointIndex", 0);

        Vector3 pos = new Vector3(
            PlayerPrefs.GetFloat("CheckpointX"),
            PlayerPrefs.GetFloat("CheckpointY"),
            PlayerPrefs.GetFloat("CheckpointZ")
        );

        player.SetPositionAndRotation(pos, Quaternion.LookRotation(Vector3.forward));

        switch (checkpointIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
        }
    }

    public void Death()
    {
        StartCoroutine(Dying());
    }
    public IEnumerator Dying()
    {
        Player.instance.inputLocked = true;
        Player.instance.StopMovement();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        canvasGroupFade.Fade(0f, 1f, 1f);

        yield return new WaitForSeconds(1.5f);
        Load();
        yield return new WaitForSeconds(1f);
        canvasGroupFade.Fade(1f, 0f, 1f);
        yield return new WaitForSeconds(1f);
        UnlockPlayer();  
    }
}