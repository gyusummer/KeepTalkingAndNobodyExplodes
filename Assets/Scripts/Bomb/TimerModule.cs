using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TimerModule : MonoBehaviour
{
    public Bomb bomb;
    public GameObject[] strikeCounter;
    
    [SerializeField]private TMP_Text timeText;
    private Stopwatch timer = new Stopwatch();
    private TimeSpan limitTime;

    public string leftTimeString;

    private void Start()
    {
        foreach (GameObject go in strikeCounter)
        {
            go.SetActive(false);
        }
        StartTimer();
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void StartTimer()
    {
        limitTime = bomb.Info.LimitTime;
        timer.Restart();
    }
    public void StopTimer()
    {
        timer.Stop();
    }

    private void UpdateTimer()
    {
        TimeSpan leftTime = limitTime - timer.Elapsed;
        if (leftTime < TimeSpan.Zero)
        {
            timer.Stop();
            leftTime = TimeSpan.Zero;
            bomb.Explode();
        }
        if (leftTime.Minutes > 0)
        {
            leftTimeString = $"{leftTime.Minutes:D2}:{leftTime.Seconds:D2}";
        }
        else
        {
            int milliseconds = (int)(leftTime.Milliseconds * 0.1f);
            leftTimeString = $"{leftTime.Seconds:D2}:{milliseconds:D2}";
        }
        timeText.text = leftTimeString;
    }
}
