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

    [Header("Light Flicker SFX")]
    [SerializeField] AudioClip[] lightFlickerClips;

    [Header("Light Gone Out SFX")]
    [SerializeField] AudioClip[] brokenLightsClips;

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
    public void PlayButtonClickSFX()
    {
        int randomIndex = Random.Range(0, buttonClicksClips.Length);
        sfxSource.PlayOneShot(buttonClicksClips[randomIndex]);
    }
    public void PlayDialogueBeepSFX()
    {
        int randomIndex = Random.Range(0, dialogueClips.Length);
        sfxSource.PlayOneShot(dialogueClips[randomIndex]);
    }

    public void PlayMonsterSFX()
    {
        int randomIndex = Random.Range(0, monsterClips.Length);
        sfxSource.PlayOneShot(monsterClips[randomIndex]);
    }

    public void PlayFootstepsSFX()
    {
        int randomIndex = Random.Range(0, footstepsClips.Length);
        sfxSource.PlayOneShot(footstepsClips[randomIndex]);
    }

    public void PlayCameraOnSFX()
    {
        sfxSource.PlayOneShot(cameraClips[0]);
    }
    public void PlayCameraOffSFX()
    {
        sfxSource.PlayOneShot(cameraClips[1]);
    }
    public void PlayCameraStartPanningSFX(AudioSource src)
    {
        src.clip = cameraClips[2];
        src.Play();
    }

    public void StopCameraStopPanningSFX(AudioSource src)
    {
        src.Stop();
    }

    public void PlayTVSFX()
    {
        int randomIndex = Random.Range(0, tvClips.Length);
        sfxSource.PlayOneShot(tvClips[randomIndex]);
    }

    public void PlayWallColliderSFX(AudioSource src)
    {
        int randomIndex = Random.Range(0, wallColliderClips.Length);

        if (src == null) src = sfxSource;
        src.PlayOneShot(wallColliderClips[randomIndex]);
    }

    public void PlayLightFlickerSFX(AudioSource src)
    {
        int randomIndex = Random.Range(0, lightFlickerClips.Length);

        if (src == null) src = sfxSource;
        src.PlayOneShot(lightFlickerClips[randomIndex], 0.2f);
    }

    public void PlayLightGoneOutSFX(AudioSource src)
    {
        int randomIndex = Random.Range(0, brokenLightsClips.Length);

        if (src == null) src = sfxSource;
        src.PlayOneShot(brokenLightsClips[randomIndex], 0.2f);
    }
}
