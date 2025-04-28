using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using UnityEngine;

public class TutorialModule : MonoBehaviour, ISelectable
{
    public TutorialBomb bomb;
    private Collider selectCollider;
    private Outlinable outline;
    [SerializeField] private AudioClip outlineTick;

    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private Transform originalParent;

    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public Collider Collider => selectCollider;

    private void Start()
    {
        outline = GetComponentInChildren<Outlinable>();
        outline.enabled = false;
        selectCollider = GetComponent<Collider>();
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
        ControlTutorialManager.Instance.audio.PlayOneShot(outlineTick);
        outline.enabled = true;
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
    }
}
