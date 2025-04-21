using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger instance;
    public static SceneChanger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneChanger>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(SceneChanger).Name;
                    instance = obj.AddComponent<SceneChanger>();
                }
            }
            return instance;
        }
    }
    
    public BombInfo BombInfo;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    public void LoadFacilityScene(TimeSpan limitTime, int moduleCount, int strikeCount)
    {
        BombInfo info = new BombInfo();
        info.LimitTime = limitTime;
        info.ModuleCount = moduleCount;
        info.StrikeCount = strikeCount;
        BombInfo = info;
        ChangeScene("Facility");
    }
}