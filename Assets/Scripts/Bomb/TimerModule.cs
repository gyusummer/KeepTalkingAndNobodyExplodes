using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class TimerModule : MonoBehaviour
{
    public Bomb Bomb;
    public GameObject[] StrikeCounter;
    
    [SerializeField]private AudioSource audio;
    [SerializeField]private TMP_Text timeText;
    private Stopwatch timer = new Stopwatch();
    private TimeSpan limitTime;
    private Coroutine coroutine;

    public string LeftTimeString;

    private void Start()
    {
        foreach (GameObject go in StrikeCounter)
        {
            go.SetActive(false);
        }
        limitTime = Bomb.Info.LimitTime;
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
            Bomb.Explode(null);
        }
        if (leftTime.Minutes > 0)
        {
            LeftTimeString = $"{leftTime.Minutes:D2}:{leftTime.Seconds:D2}";
        }
        else
        {
            int milliseconds = (int)(leftTime.Milliseconds * 0.1f);
            LeftTimeString = $"{leftTime.Seconds:D2}:{milliseconds:D2}";
        }
        timeText.text = LeftTimeString;
    }
}
