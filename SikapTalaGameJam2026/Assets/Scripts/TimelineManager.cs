using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public PlayableDirector asylumPlayableDirector;
    public void EndCutsceneDialogue()
    {
        StartCoroutine(Dialogue());
    }
    public IEnumerator Dialogue()
    {
        asylumPlayableDirector.Play();
        playableDirector.Pause();
        yield return DialogueManager.instance.PlayDialogue(new string[]
        {
            "Subject 04 has been successfully contained.",
            "The patient fled the facility under the belief that he was being pursued by a monstrous presence.",
            "Curiously, he reported that the entity was invisible to the naked eye.",
            "He claimed it could only be seen through surveillance cameras and television monitors.",
            "In reality, the figure he feared was nothing more than the distorted image of the medical staff pursuing him.",
            "He did not see things as they were.",
            "He saw them as his illness shaped them to be.",
            "The patient has been sedated, restrained, and returned to observation.",
            "Treatment will continue."
        });
        playableDirector.Resume();
    }
    public void TransitionToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
