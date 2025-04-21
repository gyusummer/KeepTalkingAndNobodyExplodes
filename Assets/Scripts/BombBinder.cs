using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BombBinder : MonoBehaviour, ISelectable
{
    public StageGroupSAO StageGroup;

    public GameObject StageSelectionPage;
    public GameObject MissionDetailPage;

    public TMP_Text txtIdentifier;
    public TMP_Text txtDescription;
    public TMP_Text txtTimeLimit;
    public TMP_Text txtModuleCount;
    public TMP_Text txtStrikeCount;

    public Animator animator;
    public Outlinable outline;
    public Collider selectCollider;

    private StageInfoSAO info;
    
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private void Start()
    {
        selectCollider = GetComponent<Collider>();
        outline.enabled = false;
    }

    public void SetMissionDetailPage(string key)
    {
        info = StageGroup.GetStageInfoOrNull(key);
        if (info == null)
        {
            Debug.LogError($"StageInfo with key {key} not found in StageGroup.");
            return;
        }
        StageSelectionPage.SetActive(false);
        
        txtIdentifier.text = info.Identifier;
        txtDescription.text = info.Description;
        txtTimeLimit.text = $"{info.LimitTimeMiniute:D2}:{info.LimitTimeSecond:D2}";
        txtModuleCount.text = $"{info.Modules} Modules";
        txtStrikeCount.text = $"{info.Strikes} Strikes";
        
        MissionDetailPage.SetActive(true);
    }
    public void BackToStageSelectionPage()
    {
        MissionDetailPage.SetActive(false);
        StageSelectionPage.SetActive(true);
        info = null;
    }

    public void StartStage()
    {
        TimeSpan limitTime = new TimeSpan(0, info.LimitTimeMiniute, info.LimitTimeSecond);
        SceneChanger.Instance.LoadFacilityScene(limitTime, info.Modules, info.Strikes);
    }

    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public Collider Collider => selectCollider;
    public ISelectable OnSelected(Transform selectPosition)
    {
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;

        animator.SetTrigger("Open");
        
        var targetPosition = selectPosition.position + selectPosition.up * 0.4f;
        
        transform.DOMove(targetPosition, 0.5f);
        transform.DORotate(selectPosition.eulerAngles, 0.5f);
        Debug.Log($"Selected ::: {gameObject.name}");
        Collider.enabled = false;
        return this;
    }

    public ISelectable OnDeselected()
    {
        BackToStageSelectionPage();
        animator.SetTrigger("Close");
        transform.DOMove(originalPosition, 0.5f);
        transform.DORotate(originalRotation, 0.5f);
        Collider.enabled = true;
        Debug.Log($"DeSelected ::: {gameObject.name}");
        return null;
    }
    public void OnMouseEnter()
    {
        outline.enabled = true;
    }

    public void OnMouseExit()
    {
        outline.enabled = false;
    }
}
