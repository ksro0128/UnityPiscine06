using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text timerText;
    public static Timer instance;
    private float time = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void StartTimer()
    {
        StartCoroutine(UpdateTimer());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    IEnumerator UpdateTimer()
    {
        while (true)
        {
            time += Time.deltaTime;
            int minutes = (int)(time / 60);
            int seconds = (int)(time % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }
    }

    public float GetTime()
    {
        return time;
    }

    public string GetTimeString()
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
