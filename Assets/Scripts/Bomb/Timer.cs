using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerModule : MonoBehaviour
{
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
            leftTimeString = $"{leftTime.Minutes:D2}:{leftTime.Seconds:D2}";
        }
        else
        {
            leftTimeString = $"{leftTime.Seconds:D2}:{leftTime.Milliseconds:D2}";
        }
        timeText.text = leftTimeString;
    }
}
