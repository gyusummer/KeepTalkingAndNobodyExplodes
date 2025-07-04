using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[Serializable]
public class PageGroup
{
    public GameObject gameObject;
    public List<TMP_Text> contentTexts;

    public void FillTexts(params string[] texts)
    {
        for (int i = 0; i < contentTexts.Count; i++)
        {
            if (i < texts.Length)
            {
                contentTexts[i].text = texts[i];
            }
            else
            {
                contentTexts[i].text = string.Empty;
            }
        }
    }
}

[RequireComponent(typeof(PointHighlighter))]
public class BombBinder : MonoBehaviour, ISelectable
{
    public StageGroupSAO StageGroup;

    public PageGroup DialogTutorialPage;
    public PageGroup ControlTutorialPage;
    public PageGroup StageSelectionPage;
    public PageGroup MissionDetailPage;
    public PageGroup ResultDefusedPage;
    public PageGroup ResultExplodedPage;

    public Animator animator;
    public Collider selectCollider;

    private PageGroup currentPage;
    private StageInfoSAO selectedStage;
    
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    [SerializeField]private float animationDuration = 0.5f;

    private void Awake()
    {
        selectCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        ShowStageSelectionPage();
    }

    public void ShowStageDetailPage(string key)
    {
        selectedStage = StageGroup.GetStageInfoOrNull(key);
        if (selectedStage == null)
        {
            Debug.LogError($"StageInfo with key {key} not found in StageGroup.");
            return;
        }
        SetPageDetail(MissionDetailPage, selectedStage);
        ShowPage(MissionDetailPage);
    }
    public void ShowStageSelectionPage()
    {
        ShowPage(StageSelectionPage);
        selectedStage = null;
    }
    public void ShowDialogTutorialPage()
    {
        ShowPage(DialogTutorialPage);
        selectedStage = null;
    }
    public void StartDialogTutorialPage()
    {
        SceneChanger.Instance.ChangeScene("DialogTutorial");
    }
    public void ShowControlTutorialPage()
    {
        ShowPage(ControlTutorialPage);
        selectedStage = null;
    }
    public void StartControlTutorialPage()
    {
        SceneChanger.Instance.ChangeScene("ControlTutorial");
    }
    public void ShowResultPage(ResultInfo info)
    {
        if (info.isDefused)
        {
            SetPageDetail(ResultDefusedPage, info.ToStringArray());
            ShowPage(ResultDefusedPage);
        }
        else
        {
            SetPageDetail(ResultExplodedPage, info.ToStringArray());
            ShowPage(ResultExplodedPage);
        }
    }
    private void SetPageDetail(PageGroup pageGroup, params string[] texts)
    {
        pageGroup.FillTexts(texts);
    }
    private void SetPageDetail(PageGroup pageGroup, StageInfoSAO info)
    {
        SetPageDetail(pageGroup,
            info.Identifier, 
            info.Description, 
            $"{info.LimitMinute:D2}:{info.LimitSecond:D2}", 
            $"{info.Modules} Modules", 
            $"{info.Strikes} Strikes");
    }
    private void ShowPage(PageGroup pageGroup)
    {
        if (currentPage != null)
        {
            currentPage.gameObject.SetActive(false);
        }
        pageGroup.gameObject.SetActive(true);
        currentPage = pageGroup;
    }

    public void StartStage()
    {
        if (selectedStage == null)
        {
            selectedStage = SceneChanger.Instance.currentStageInfo;
        }
        SceneChanger.Instance.LoadFacilityScene(selectedStage);
    }
    public void BackToSetupRoom()
    {
        SceneChanger.Instance.LoadSetupScene();
    }

    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public Collider Collider => selectCollider;
    public ISelectable OnSelected(Transform selectPosition)
    {
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;

        animator.SetTrigger("Open");
        
        transform.DOMove(selectPosition.position, animationDuration);
        transform.DORotate(selectPosition.eulerAngles, animationDuration);
        Camera.main.DOFieldOfView(40, animationDuration);
        // Debug.Log($"Selected ::: {gameObject.name}");
        Collider.enabled = false;
        return this;
    }

    public ISelectable OnDeselected()
    {
        ShowStageSelectionPage();
        animator.SetTrigger("Close");
        transform.DOMove(originalPosition, animationDuration);
        transform.DORotate(originalRotation, animationDuration).onComplete += () =>
        {
            Collider.enabled = true;
        };
        Camera.main.DOFieldOfView(60, animationDuration);
        // Debug.Log($"DeSelected ::: {gameObject.name}");
        return null;
    }

    public void PlaySound(AudioClip clip)
    {
        AudioManager.Instance.PlaySfx(clip);
    }
}
