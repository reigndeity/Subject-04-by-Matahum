using UnityEngine;

public class WallColliderObstacle : MonoBehaviour
{
    AudioSource src;
    Animator anim;
    public float animInterval;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
        if (src == null) src = gameObject.AddComponent<AudioSource>();

        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke("StartAnimation", animInterval);
    }

    public void StartAnimation()
    {
        anim.Play("New Closing Walls Anim");
    }
    public void PlayWallColliderSFX()
    {
        AudioManager.instance.PlayWallColliderSFX(src);
    }
}
