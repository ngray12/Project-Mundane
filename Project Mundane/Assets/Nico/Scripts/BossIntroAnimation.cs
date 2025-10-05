using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntroAnimation : MonoBehaviour
{
    public Animator bossAnimator;
    public string introTrigger;
    public float introDuration;
    public MonoBehaviour bossScript;
    public MonoBehaviour playerScript;

    void Awake()
    {
        if (bossScript != null)
        {
            bossScript.enabled = false;
        }

        if (playerScript != null)
        {
            playerScript.enabled = false;
        }

        if(bossAnimator != null)
        {
            bossAnimator.SetTrigger(introTrigger);
        }

        Invoke(nameof(FinishIntro), introDuration);
    }

    void FinishIntro()
    {
        if (bossScript != null)
        {
            bossScript.enabled = true;
        }

        if (playerScript != null)
        {
            playerScript.enabled = true;
        }
    }
}
