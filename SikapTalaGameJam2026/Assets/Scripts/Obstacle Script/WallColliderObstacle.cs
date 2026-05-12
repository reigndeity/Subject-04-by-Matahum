using UnityEngine;

public class WallColliderObstacle : MonoBehaviour
{
    AudioSource src;

    private void Awake()
    {
        src = GetComponent<AudioSource>();

        if (src == null) src = gameObject.AddComponent<AudioSource>();
    }
    public void PlayWallColliderSFX()
    {
        AudioManager.instance.PlayWallColliderSFX(src);
    }
}
