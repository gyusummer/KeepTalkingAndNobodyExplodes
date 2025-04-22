using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    
    private void Awake()
    {
        Instance = this;
    }

    public void ShowResult(ResultInfo info)
    {
        binder.ShowResultPage(info);
        
        Controller.Instance.transform.position = transform.position;
        Controller.Instance.transform.rotation = transform.rotation;
        Controller.Instance.Select(binder);
    }
}
