using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    [SerializeField] AudioSource musicSource;

    [Header("Sound Effects")]
    [SerializeField] AudioSource sfxSource;

    [Header("Music Clips")]
    [SerializeField] AudioClip[] musicClips;

    [Header("Button Click Clips")]
    [SerializeField] AudioClip[] buttonClicksClips;

    [Header("Dialogue Clips")]
    [SerializeField] AudioClip[] dialogueClips;

    [Header("Monster SFX")]
    [SerializeField] AudioClip[] monsterClips;

    [Header("Footsteps SFX")]
    [SerializeField] AudioClip[] footstepsClips;

    [Header("CCTV SFX")]
    [SerializeField] AudioClip[] cameraClips;

    [Header("TV SFX")]
    [SerializeField] AudioClip[] tvClips;

    [Header("Wall Collider SFX")]
    [SerializeField] AudioClip[] wallColliderClips;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    public void PlayDialogueBeepSFX()
    {
        int randomIndex = Random.Range(0, dialogueClips.Length);
        sfxSource.PlayOneShot(dialogueClips[randomIndex]);
    }

    public void PlayMonsterSFX()
    {
        int randomIndex = Random.Range(0, dialogueClips.Length);
        sfxSource.PlayOneShot(dialogueClips[randomIndex]);
    }

    public void PlayFootstepsSFX()
    {
        int randomIndex = Random.Range(0, dialogueClips.Length);
        sfxSource.PlayOneShot(dialogueClips[randomIndex]);
    }

    public void PlayCameraSFX()
    {
        int randomIndex = Random.Range(0, dialogueClips.Length);
        sfxSource.PlayOneShot(dialogueClips[randomIndex]);
    }

    public void PlayTVSFX()
    {
        int randomIndex = Random.Range(0, dialogueClips.Length);
        sfxSource.PlayOneShot(dialogueClips[randomIndex]);
    }

    public void PlayWallColliderSFX()
    {
        int randomIndex = Random.Range(0, dialogueClips.Length);
        sfxSource.PlayOneShot(dialogueClips[randomIndex]);
    }
}
