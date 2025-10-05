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
    public Rigidbody2D rbBoss;
    public Rigidbody2D rbPlaer;

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

        if (rbBoss != null)
        {
            rbBoss.gravityScale = 0;
        }

        if (rbPlaer != null)
        {
            rbPlaer.gravityScale = 0;
        }

        if (bossAnimator != null)
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

        if (rbBoss != null)
        {
            rbBoss.gravityScale = 1;
        }

        if (rbPlaer != null)
        {
            rbPlaer.gravityScale = 1;
        }
    }
}