using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Manager : MonoBehaviour
{
    public string nextScene;

    [Header("Main Menu Buttons")]
    public Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(NextScene);
    }
    public void NextScene()
    {
        AudioManager.instance.PlayButtonClickSFX();
        SceneManager.LoadScene(nextScene);
    }
}
