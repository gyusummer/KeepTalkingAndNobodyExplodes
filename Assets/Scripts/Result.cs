using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UI;
using UnityEngine;

public class ResultInfo
{
    public bool isDefused;
    public StageInfoSAO stageInfo;
    public string leftTimeString;
    public string causeOfExplosion;

    public string[] ToStringArray()
    {
        string[] result = new string[6];
        result[0] = stageInfo.Identifier;
        result[1] = $"{stageInfo.LimitTimeMiniute:D2}:{stageInfo.LimitTimeSecond:D2}";
        result[2] = $"{stageInfo.Modules} Modules";
        result[3] = $"{stageInfo.Strikes} Strikes";
        result[4] = $"Time Remaining:\n{leftTimeString}";
        result[5] = isDefused ? String.Empty : $"Cause of Explosion:\n{causeOfExplosion}";
        return result;
    }
}
public class Result : MonoBehaviour
{
    public static Result Instance;
    public BombBinder binder;
    public AudioSource audio;
    public AudioClip fanfare;
    public AudioClip lose;
    public AudioClip win;

    public Canvas canvas;
    public Image black;
    
    private void Awake()
    {
        Instance = this;
    }

    public void ShowResult(ResultInfo info)
    {
        binder.ShowResultPage(info);

        if (info.isDefused == true)
        {
            StartCoroutine(Win_Coroutine());
        }
        else
        {
            black.color = Color.black;
            black.gameObject.SetActive(true);
            CommonRoutine();
        }
    }

    private IEnumerator Win_Coroutine()
    {
        yield return new WaitForSeconds(2.0f);
        audio.PlayOneShot(fanfare);
        yield return new WaitForSeconds(3.0f);
        black.color = Color.clear;
        black.gameObject.SetActive(true);
        black.DOFade(1.0f, 1.0f).onComplete += CommonRoutine;
    }
    private void CommonRoutine()
    {
        Controller.Instance.transform.position = transform.position;
        canvas.planeDistance = 3.0f;
        binder.gameObject.SetActive(true);
        Controller.Instance.Select(binder);
    }
}
