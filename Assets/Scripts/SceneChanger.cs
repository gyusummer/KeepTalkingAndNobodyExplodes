using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    public event Action OnScneChange;
    public Image black;
    
    public StageInfoSAO currentStageInfo;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            black.DOFade(0.0f, 1.0f).OnComplete(() =>
            {
                black.gameObject.SetActive(false);
            });
        };
    }

    public void ChangeScene(string sceneName)
    {
        black.color = Color.clear;
        black.gameObject.SetActive(true);
        OnScneChange?.Invoke();
        black.DOFade(1.0f, 1.0f).onComplete += () =>
        {
            SceneManager.LoadSceneAsync(sceneName);
        };
    }
    public void LoadFacilityScene(StageInfoSAO stageInfo)
    {
        currentStageInfo = stageInfo;
        ChangeScene("Facility");
    }
    public void LoadSetupScene()
    {
        ChangeScene("Setup");
    }
}