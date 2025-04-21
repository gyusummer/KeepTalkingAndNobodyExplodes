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

    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private Transform rootBomb;
    private Transform originalParent;
    protected abstract void Disarm();

    protected void EssentialInit()
    {
        statusLED = GetComponentInChildren<StatusLight>();
        selectCollider = GetComponent<Collider>();
        outline = GetComponent<Outlinable>();
        outline.enabled = false;
    }

    public virtual ISelectable OnSelected(Transform selectPosition)
    {
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;
        
        rootBomb = transform.root;
        originalParent = transform.parent;
        transform.parent = null;
        rootBomb.parent = transform;
        
        var targetPosition = selectPosition.position + selectPosition.up * 0.1f;
        
        transform.DOMove(targetPosition, 0.5f);
        var tween = transform.DORotate(selectPosition.eulerAngles, 0.5f);
        tween.onComplete += () =>
        {
            rootBomb.parent = null;
            transform.parent = originalParent;
        };
        Collider.enabled = false;
        Camera.main.DOFieldOfView(40, 0.5f);
        
        Debug.Log($"Selected ::: {gameObject.name}");

        return this;
    }

    public virtual ISelectable OnDeselected()
    {
        rootBomb = transform.root;
        originalParent = transform.parent;
        transform.parent = null;
        rootBomb.parent = transform;
        
        transform.DOMove(originalPosition, 0.5f);
        var tween = transform.DORotate(originalRotation, 0.5f);
        tween.onComplete += () =>
        {
            rootBomb.parent = null;
            transform.parent = originalParent;
        };
        Collider.enabled = true;
        Camera.main.DOFieldOfView(60, 0.5f);
        
        Debug.Log($"DeSelected ::: {gameObject.name}");

        return rootBomb.GetComponent<ISelectable>();
    }
    private void OnMouseEnter()
    {
        outline.enabled = true;
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
    }
}
