using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class TimerModule : MonoBehaviour
{
    [SerializeField]private TMP_Text timeText;
    private Stopwatch timer = new Stopwatch();
    private TimeSpan limitTime;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void StartTimer()
    {
        limitTime = Bomb.Instance.LimitTime;
        timer.Restart();
    }

    private void UpdateTimer()
    {
        TimeSpan leftTime = limitTime - timer.Elapsed;
        if (leftTime < TimeSpan.Zero)
        {
            timer.Stop();
            leftTime = TimeSpan.Zero;
        }
        if (leftTime.Minutes > 0)
        {
            timeText.text = $"{leftTime.Minutes:D2}:{leftTime.Seconds:D2}";
        }
        else
        {
            timeText.text = $"{leftTime.Seconds:D2}:{leftTime.Milliseconds:D2}";
        }
    }
}
