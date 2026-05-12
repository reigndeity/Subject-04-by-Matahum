using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Manager : MonoBehaviour
{
    public CanvasGroupFade canvasGroupFade;
    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;

    [Header("Settings Properties")]
    public TextMeshProUGUI currentSensitivityText;
    public Button backToMenuFromSettingsButton;
    public Slider sensitivitySlider;
    [Header("Exit Buttons")]
    public Button cancelExitButton;
    public Button confirmExitButton;
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject exitPanel;

    private void Awake()
    {
        startButton.onClick.AddListener(OnClickStart);
        settingsButton.onClick.AddListener(OnClickSettings);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);
        exitButton.onClick.AddListener(OnClickExit);
        backToMenuFromSettingsButton.onClick.AddListener(OnClickBackToMenuFromSettings);
        cancelExitButton.onClick.AddListener(OnClickCancelExit);
        confirmExitButton.onClick.AddListener(OnClickConfirmExt);
    }

    void Start()
    {
        sensitivitySlider.minValue = 0.01f;
        sensitivitySlider.maxValue = 2f;

        sensitivitySlider.onValueChanged.RemoveListener(OnSensitivitySliderChanged);

        if (!PlayerPrefs.HasKey("MouseSensitivity"))
            PlayerPrefs.SetFloat("MouseSensitivity", 1f);

        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        sensitivitySlider.value = savedSensitivity;

        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);

        UpdateSensitivityText();

        canvasGroupFade.Fade(1f, 0f, 1f);
    }
    public void OnClickStart()
    {
        StartCoroutine(Starting());
    }
    IEnumerator Starting()
    {
        AudioManager.instance.PlayButtonClickSFX();
        canvasGroupFade.Fade(0f, 1f, 1f);
        startButton.interactable = false;
        settingsButton.interactable = false;
        exitButton.interactable = false;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickSettings()
    {
        AudioManager.instance.PlayButtonClickSFX();
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void OnClickBackToMenuFromSettings()
    {
        AudioManager.instance.PlayButtonClickSFX();
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void OnSensitivitySliderChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        UpdateSensitivityText();
    }

    void UpdateSensitivityText()
    {
        currentSensitivityText.text = PlayerPrefs.GetFloat("MouseSensitivity", 1f).ToString("F2");
    }

    public void OnClickExit()
    {
        AudioManager.instance.PlayButtonClickSFX();
        mainMenuPanel.SetActive(false);
        exitPanel.SetActive(true);
    }
    public void OnClickConfirmExt()
    {
        AudioManager.instance.PlayButtonClickSFX();
        Application.Quit();
    }
    public void OnClickCancelExit()
    {
        AudioManager.instance.PlayButtonClickSFX();
        exitPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
    