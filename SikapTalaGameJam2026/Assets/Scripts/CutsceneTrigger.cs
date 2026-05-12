using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    private PlayableDirector playableDirector;
    public PlayableAsset cutscene;
    void Start()
    {
        playableDirector = GameManager.instance.playableDirector;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerCutscene(cutscene);
            this.gameObject.SetActive(false);
        }
    }

    public void TriggerCutscene(PlayableAsset cutscene)
    {
        Player.instance.inputLocked = true;
        Player.instance.StopMovement();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playableDirector.playableAsset = cutscene;
        playableDirector.Play();
    }
}
