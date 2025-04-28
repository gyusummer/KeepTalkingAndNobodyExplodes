using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using EPOOutline;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TutorialBomb : MonoBehaviour, ISelectable
{
    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public Collider Collider => selectCollider;
    private Collider selectCollider;
    
    private Outlinable outline;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip outlineTick;
    public Vector3 originalPosition;
    public Vector3 originalRotation;

    private void Start()
    {
        selectCollider = GetComponent<Collider>();
        outline = GetComponent<Outlinable>();

        outline.enabled = false;
    }
    public ISelectable OnSelected(Transform selectPosition)
    {
        ControlTutorialManager.Instance.BombEvent("BombSelected", this);
        
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;
        
        var targetPosition = selectPosition.position - selectPosition.up * 0.1f;

        transform.DOMove(targetPosition, 0.5f);
        transform.DORotate(selectPosition.eulerAngles, 0.5f);
        Debug.Log($"Selected ::: {gameObject.name}");
        Collider.enabled = false;
        return this;
    }

    public ISelectable OnDeselected()
    {
        ControlTutorialManager.Instance.BombEvent("BombDeselected");

        Debug.Log($"DeSelected ::: {gameObject.name}");
        transform.DOMove(originalPosition, 0.5f);
        transform.DORotate(originalRotation, 0.5f);
        Collider.enabled = true;
        return null;
    }

    private void OnMouseEnter()
    {
        audio.PlayOneShot(outlineTick);
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }
}