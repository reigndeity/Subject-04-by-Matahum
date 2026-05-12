using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayIdleAnim()
    {
        PlayAnimation(0);
    }

    public void PlayJogAnim()
    {
        PlayAnimation(1);
    }

    public void PlayJumpAnim()
    {
        PlayAnimation(2);
    }

    public void PlayFallAnim()
    {
        PlayAnimation(3);
    }

    public void PlayLandingAnim()
    {
        PlayAnimation(4);
    }

    void PlayAnimation(int index)
    {
        /*if (IsPlaying(animName))
            return;*/

        anim.SetInteger("PlayerAnimation", index);
    }

    bool IsPlaying(string animName)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        return stateInfo.IsName(animName);
    }
}