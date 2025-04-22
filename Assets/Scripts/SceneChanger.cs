using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    
    public BombInfo BombInfo;
    public StageInfoSAO currentStageInfo;

    private void Awake()
    {
        Instance = this;
    }
    private void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    public void LoadFacilityScene(StageInfoSAO stageInfo)
    {
        currentStageInfo = stageInfo;
        BombInfo info = new BombInfo();
        TimeSpan limitTime = new TimeSpan(0, stageInfo.LimitTimeMiniute, stageInfo.LimitTimeSecond);
        info.LimitTime = limitTime;
        info.ModuleCount = stageInfo.Modules;
        info.StrikeCount = stageInfo.Strikes;
        BombInfo = info;
        ChangeScene("Facility");
    }
    public void LoadSetupScene()
    {
        ChangeScene("Setup");
    }
}