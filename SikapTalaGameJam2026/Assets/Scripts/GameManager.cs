using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CanvasGroupFade canvasGroupFade;
    public CanvasGroupFade savingCanvasGroupFade;
    public int checkpointIndex = 0;

    public Transform player;
    [Header("Game Over")]
    public GameObject gameOverPanel;
    public Button confirmRestartButton;
    public Button cancelRestartButton;
    public PlayableAsset uiGameOver;
    public TextMeshProUGUI restartFromLastCheckpointTxt;
    public GameObject gameOverTitleObj;
    [Header("Cutscenes")]
    public PlayableDirector playableDirector;
    public PlayableAsset cutscene1;

    [Header("Tutorials")]
    public GameObject movementTutorial;
    public GameObject jumpTutorial;
    public GameObject mouseTutorial;

    [Header("Start Game")]
    public Monster monster;

    [Header("TV Optimizer")]
    public TV_Opt[] tv_Opt;

    [Header("Light Manager")]
    public LightManager lightManager;

    [Header("Character Properties")]
    public GameObject playerObj;

    void Awake()
    {
        instance = this;
        playableDirector.playableAsset = null;
    }

    void Start()
    {
        StartCoroutine(StartGame());
        confirmRestartButton.onClick.AddListener(ConfirmRestart);
        cancelRestartButton.onClick.AddListener(ConfirmCancelRestart);
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

        StartCoroutine(Saving());
    }
    IEnumerator Saving()
    {
        savingCanvasGroupFade.Fade(0f, 1f, 0.5f);
        yield return new WaitForSeconds(1f);
        savingCanvasGroupFade.Fade(1f, 0f, 0.5f);
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

        player.SetPositionAndRotation(pos, Quaternion.identity);

        Player.instance.ResetCameraRotation();

        MonsterTeleporter.instance.TeleportAndMove(checkpointIndex);
        lightManager.LightReset(checkpointIndex);

        switch (checkpointIndex)
        {
            case 0:
                OptimizeTV(0, true);
                break;

            case 3:
                OptimizeTV(1, true);
                OptimizeTV(0, false);
                break;

            case 5:
                OptimizeTV(1, false);
                break;

            case 6:
                OptimizeTV(2, true);
                break;

            case 7:
                OptimizeTV(3, true);
                break;
        }

    }

    public void Death()
    {
        StartCoroutine(Dying());
    }
    public void ConfirmRestart()
    {
        StartCoroutine(Restarting());
        AudioManager.instance.PlayButtonClickSFX();
    }
    public void ConfirmCancelRestart()
    {
        StartCoroutine(CancellingRestart());
        AudioManager.instance.PlayButtonClickSFX();
    }
    public IEnumerator Dying()
    {
        monster.canMove = false;
        Player.instance.inputLocked = true;
        Player.instance.StopMovement();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canvasGroupFade.Fade(0f, 1f, 0.5f);
        restartFromLastCheckpointTxt.text = "Restart from last checkpoint?";
        confirmRestartButton.gameObject.SetActive(false);
        cancelRestartButton.gameObject.SetActive(false);
        restartFromLastCheckpointTxt.gameObject.SetActive(false);
        gameOverTitleObj.gameObject.SetActive(false);
        playerObj.tag = "Untagged";
        yield return new WaitForSeconds(0.5f);
        gameOverPanel.SetActive(true);
        canvasGroupFade.Fade(1f, 0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        confirmRestartButton.GetComponent<TextMeshProUGUI>().text = "YES";
        cancelRestartButton.GetComponent<TextMeshProUGUI>().text = "NO";
        playableDirector.Play(uiGameOver);  
    }
    public IEnumerator Restarting()
    {
        canvasGroupFade.Fade(0f, 1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        playableDirector.playableAsset = null;  
        gameOverPanel.SetActive(false);
        confirmRestartButton.GetComponent<TextMeshProUGUI>().text = "";
        cancelRestartButton.GetComponent<TextMeshProUGUI>().text = "";
        restartFromLastCheckpointTxt.text = "";
        Load();
        playerObj.tag = "Player";
        yield return new WaitForSeconds(1f);
        canvasGroupFade.Fade(1f, 0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        monster.canMove = true;
        UnlockPlayer();
    }
    public IEnumerator CancellingRestart()
    {
        canvasGroupFade.Fade(0f, 1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenuScene");
    }

    public void PlayDialogueBeep()
    {
        AudioManager.instance.PlayDialogueBeepSFX();
    }

    public void EnableMonster()
    {
        monster.gameObject.SetActive(true);
        monster.canMove = true;
    }

    public void OptimizeTV(int index, bool toggleTV)
    {
        int count = tv_Opt[index].tv_Opt.Length;

        for (int i = 0; i < count; i++)
        {
            tv_Opt[index].tv_Opt[i].SetCameras(toggleTV);
        }
    }

    public void EndGame()
    {
        StartCoroutine(EndingGame());
    }
    IEnumerator EndingGame()
    {
        Player.instance.inputLocked = true;
        Player.instance.StopMovement();
        canvasGroupFade.Fade(0f, 1f, 0.5f);
        monster.canMove = false;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("EndCutscene");
    }
}

[System.Serializable]
public class TV_Opt
{
    public TV_Optimizer[] tv_Opt;
}