using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLoader : MonoBehaviour
{
    public float timeTillBoss = 50f;
    public string bossSceneName = "Square Boss Scene";

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeTillBoss)
        {
            SceneManager.LoadScene(bossSceneName);
        }
    }
}
