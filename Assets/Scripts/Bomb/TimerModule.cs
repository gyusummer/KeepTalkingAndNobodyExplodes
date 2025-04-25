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
    
    [SerializeField]private AudioSource audio;
    [SerializeField]private TMP_Text timeText;
    private Stopwatch timer = new Stopwatch();
    private TimeSpan limitTime;
    private Coroutine coroutine;

    public string leftTimeString;

    private void Start()
    {
        foreach (GameObject go in strikeCounter)
        {
            go.SetActive(false);
        }
        limitTime = bomb.Info.LimitTime;
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void StartTimer()
    {
        coroutine = StartCoroutine(Beep_Coroutine());
        timer.Restart();
    }

    private IEnumerator Beep_Coroutine()
    {
        var wfs = new WaitForSeconds(1f);
        while (gameObject.activeInHierarchy)
        {
            audio.Play();
            yield return wfs;
        }
    }
    public void StopTimer()
    {
        audio.clip = null;
        StopCoroutine(coroutine);
        timer.Stop();
    }

    private void UpdateTimer()
    {
        TimeSpan leftTime = limitTime - timer.Elapsed;
        if (leftTime < TimeSpan.Zero)
        {
            timer.Stop();
            leftTime = TimeSpan.Zero;
            bomb.Explode(null);
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
