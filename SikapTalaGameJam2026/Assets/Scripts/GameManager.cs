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

    [Header("Pause Properties")]
    public GameObject pausePanel;
    public GameObject menuPanel;
    public Button pauseResumeButton;
    public Button pauseMainMenuButton;
    public Slider mouseSensitivitySlider;
    public TextMeshProUGUI mouseSensitivityValueText;
    public float currentMouseSensitivity;
    public Button confirmMenuButton;
    public Button cancelMenuButton;

    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool isCCTVActive = false;

    private CanvasGroup fadeCanvasGroup;

    void Awake()
    {
        instance = this;
        playableDirector.playableAsset = null;

        if (canvasGroupFade != null)
        {
            fadeCanvasGroup = canvasGroupFade.GetComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        StartCoroutine(StartGame());
        confirmRestartButton.onClick.AddListener(ConfirmRestart);
        cancelRestartButton.onClick.AddListener(ConfirmCancelRestart);

        pauseResumeButton.onClick.AddListener(TogglePause);

        pauseMainMenuButton.onClick.AddListener(OnClickPauseMenu);
        confirmMenuButton.onClick.AddListener(OnClickConfirmMenu);
        cancelMenuButton.onClick.AddListener(OnClickCancelMenu);

        // Initialize Sensitivity Slider
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.minValue = 0.1f;
            mouseSensitivitySlider.maxValue = 2f;
            mouseSensitivitySlider.value = savedSensitivity;
            UpdateSensitivityText(savedSensitivity);
            mouseSensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // CHECK: Block pause menu if a dialogue sequence is active
            if (DialogueManager.instance != null && DialogueManager.instance.IsInDialogue())
                return;

            // CHECK: Block pause menu if a cutscene sequence is playing
            if (IsCutscenePlaying())
                return;

            // Do not pause if in CCTV mode or game over screen is open
            if (isCCTVActive || (gameOverPanel != null && gameOverPanel.activeInHierarchy))
                return;

            // Do not pause if the main canvas group is currently fading
            if (fadeCanvasGroup != null && fadeCanvasGroup.alpha > 0f && fadeCanvasGroup.alpha < 1f)
                return;

            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pausePanel != null)
                pausePanel.SetActive(true);

            Player.instance.inputLocked = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            if (pausePanel != null)
                pausePanel.SetActive(false);

            Player.instance.inputLocked = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        AudioManager.instance.PlayButtonClickSFX();
    }

    void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
        UpdateSensitivityText(value);

        if (Player.instance != null)
        {
            PlayerCamera playerCam = Player.instance.GetComponentInChildren<PlayerCamera>();
            if (playerCam != null)
            {
                playerCam.UpdateSensitivityFromPrefs();
            }
        }
    }

    void UpdateSensitivityText(float value)
    {
        if (mouseSensitivityValueText != null)
        {
            mouseSensitivityValueText.text = value.ToString("F2");
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
        AudioManager.instance.PlayScareSFX();
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
        AudioManager.instance.PlayDeathSFX();
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
        Time.timeScale = 1f;
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

    public void OnClickPauseMenu()
    {
        menuPanel.SetActive(true);
        pausePanel.SetActive(false);
        AudioManager.instance.PlayButtonClickSFX();
    }
    public void OnClickCancelMenu()
    {
        menuPanel.SetActive(false);
        pausePanel.SetActive(true);
        AudioManager.instance.PlayButtonClickSFX();
    }
    public void OnClickConfirmMenu()
    {
        StartCoroutine(CancellingRestart());
        AudioManager.instance.PlayButtonClickSFX();
    }
}

[System.Serializable]
public class TV_Opt
{
    public TV_Optimizer[] tv_Opt;
}