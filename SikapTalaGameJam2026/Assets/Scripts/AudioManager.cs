using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] AudioSource musicSource;

    [Header("Sound Effects")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip[] sfxClips;


    public void PlayDialogueBeepSFX()
    {
        int randomIndex = Random.Range(0, sfxClips.Length);
        sfxSource.PlayOneShot(sfxClips[randomIndex]);
    }
}
