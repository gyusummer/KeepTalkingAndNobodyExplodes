using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(PointHighlighter),typeof(AudioSource))]
public abstract class DisarmableModule : MonoBehaviour, ISelectable
{
    public Bomb Bomb;
    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public Collider Collider { get; private set; }

    private PointHighlighter highlighter;
    private StatusLight statusLed;
    protected ModulePart[] parts;

    protected PartEventInfo keyEvent;

    public Vector3 OriginalPosition;
    public Vector3 OriginalRotation;
    private Transform originalParent;
    
    private void Start()
    {
        if (Bomb == null)
        {
            Bomb = Bomb.Main;
        }
        EssentialInit();
        Init();
        SetKeyEvent();
    }
    private void EssentialInit()
    {
        Collider = GetComponent<Collider>();
        highlighter = GetComponent<PointHighlighter>();
        statusLed = GetComponentInChildren<StatusLight>();
        parts = GetComponentsInChildren<ModulePart>();
        
        keyEvent = new PartEventInfo();

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].MainEvent += Judge;
        }
    }
    protected abstract void Init();
    protected abstract void SetKeyEvent(); // 정답 설정
    protected virtual bool IsCorrectEvent(PartEventInfo partEvent)
    {
        if (keyEvent.part == partEvent.part)
        {
            return true;
        }
        return false;
    }
    private void Judge(PartEventInfo partEvent)
    {
        if (IsCorrectEvent(partEvent))
        {
            Hit(partEvent); // 정답
        }
        else
        {
            Strike(partEvent); // 오답
        }
    }
    protected virtual void Hit(PartEventInfo info)
    {
        Disarm(); // 모듈 해체
    }
    protected virtual void Strike(PartEventInfo info)
    {
        statusLed.LightRed();
        Bomb.Strike(this);
    }
    
    protected void Disarm()
    {
        statusLed.LightGreen();
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].MainEvent -= Judge;
            parts[i].enabled = false;
        }
        this.enabled = false;
        Debug.Log($"{this.GetType().Name} Disarmed");
        Bomb.CurDisarm++;
    }
    
    public virtual ISelectable OnSelected(Transform selectPosition)
    {
        OriginalPosition = transform.position;
        OriginalRotation = transform.eulerAngles;
        
        originalParent = transform.parent;
        transform.parent = null;
        Bomb.transform.parent = transform;
        
        var targetPosition = selectPosition.position + selectPosition.up * 0.1f;
        
        transform.DOMove(targetPosition, 0.5f);
        var tween = transform.DORotate(selectPosition.eulerAngles, 0.5f);
        tween.onComplete += () =>
        {
            Bomb.transform.parent = null;
            transform.parent = originalParent;
        };
        Collider.enabled = false;
        
        return this;
    }
    public virtual ISelectable OnDeselected()
    {
        originalParent = transform.parent;
        transform.parent = null;
        Bomb.transform.parent = transform;
        
        transform.DOMove(OriginalPosition, 0.5f);
        transform.DORotate(OriginalRotation, 0.5f).onComplete += () =>
        {
            Bomb.transform.parent = null;
            transform.parent = originalParent;
        };
        Collider.enabled = true;
        
        return Bomb;
    }
    
    private void OnDisable()
    {
        highlighter.enabled = false;
    }
    
    private void OnDestroy()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].MainEvent -= Judge;
        }
    }

}
