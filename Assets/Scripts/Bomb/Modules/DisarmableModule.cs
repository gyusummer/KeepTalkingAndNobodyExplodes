using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using UnityEngine;

public abstract class DisarmableModule : MonoBehaviour, ISelectable
{
    public Bomb bomb;
    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public Collider Collider => selectCollider;
    private Collider selectCollider;
    private Outlinable outline;
    protected StatusLight statusLED;
    protected ModulePart[] parts;

    protected PartEventInfo keyEvent;

    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private Transform originalParent;
    
    private void Start()
    {
        EssentialInit();
        Init();
        SetKeyEvent();
    }
    private void EssentialInit()
    {
        statusLED = GetComponentInChildren<StatusLight>();
        selectCollider = GetComponent<Collider>();
        outline = GetComponent<Outlinable>();
        parts = GetComponentsInChildren<ModulePart>();
        keyEvent = new PartEventInfo();

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].MainEvent += Judge;
        }
        outline.enabled = false;
    }
    protected abstract void Init();

    protected abstract void SetKeyEvent();
    protected virtual bool CompareKeyEvent(PartEventInfo partEvent)
    {
        if (keyEvent.part == partEvent.part)
        {
            return true;
        }
        return false;
    }
    protected virtual void Judge(PartEventInfo partEvent)
    {
        if (CompareKeyEvent(partEvent))
        {
            Success(partEvent);
        }
        else
        {
            Strike(partEvent);
        }
    }
    protected virtual void Success(PartEventInfo info)
    {
        Disarm();
    }
    protected virtual void Strike(PartEventInfo info)
    {
        statusLED.LightRed();
        bomb.Strike(this);
    }
    protected void Disarm()
    {
        statusLED.LightGreen();
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].MainEvent -= Judge;
            parts[i].enabled = false;
        }
        this.enabled = false;
        Debug.Log($"{this.GetType().Name} Disarmed");
        bomb.CurDisarm++;
    }
    public virtual ISelectable OnSelected(Transform selectPosition)
    {
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;
        
        originalParent = transform.parent;
        transform.parent = null;
        bomb.transform.parent = transform;
        
        var targetPosition = selectPosition.position + selectPosition.up * 0.1f;
        
        transform.DOMove(targetPosition, 0.5f);
        var tween = transform.DORotate(selectPosition.eulerAngles, 0.5f);
        tween.onComplete += () =>
        {
            bomb.transform.parent = null;
            transform.parent = originalParent;
        };
        Collider.enabled = false;
        
        Debug.Log($"Selected ::: {gameObject.name}");

        return this;
    }
    public virtual ISelectable OnDeselected()
    {
        originalParent = transform.parent;
        transform.parent = null;
        bomb.transform.parent = transform;
        
        transform.DOMove(originalPosition, 0.5f);
        transform.DORotate(originalRotation, 0.5f).onComplete += () =>
        {
            bomb.transform.parent = null;
            transform.parent = originalParent;
        };
        Collider.enabled = true;
        
        Debug.Log($"DeSelected ::: {gameObject.name}");

        return bomb;
    }
    private void OnMouseEnter()
    {
        outline.enabled = true;
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
    }
    protected virtual void OnDestroy()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].MainEvent -= Judge;
        }
    }
}
