using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class PartEventInfo
{
    public ModulePart part;
    public float time;
    public string parameter;

    public PartEventInfo()
    {
        part = null;
        time = 0;
        parameter = string.Empty;
    }

    public PartEventInfo(ModulePart part)
    {
        this.part = part;
    }
}
[RequireComponent(typeof(Outlinable),typeof(AudioSource))]
public abstract class ModulePart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
{
    public Action<PartEventInfo> MainEvent;
    public Action SubEvent;
    
    protected Outlinable outline;
    protected AudioSource audio;
    protected AudioClip outlineTick;
    protected AudioClip buttonDown;

    private void Start()
    {
        outline = GetComponent<Outlinable>();
        audio = GetComponent<AudioSource>();
        outlineTick = Resources.Load(StaticStrings.AudioClipPath.Tick) as AudioClip;
        Debug.Log(StaticStrings.AudioClipPath.Tick);
        Debug.Log(Resources.Load(StaticStrings.AudioClipPath.Tick));
        buttonDown = Resources.Load(StaticStrings.AudioClipPath.ButtonPress) as AudioClip;
        outline.OutlineParameters.Color = Color.red;
        outline.enabled = false;
        Init();
    }

    protected virtual void Init() {}

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        audio.clip = outlineTick;
        audio.Play();
        outline.enabled = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
    protected virtual void OnDisable()
    {
        if(outline == null) return;
        outline.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        audio.clip = buttonDown;
        audio.Play();
        OnButtonDown();
    }

    protected virtual void OnButtonDown()
    {
        MainEvent?.Invoke(new PartEventInfo(this));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        #region ClickSpringRotation
        Transform bomb = Bomb.Main.transform;
        Vector3 clickPosition = eventData.pointerCurrentRaycast.worldPosition;
        Vector3 clickVector = (clickPosition - bomb.position).normalized;

        float quadrant = Vector3.Dot(clickVector, bomb.right) * Vector3.Dot(clickVector, bomb.forward);

        Vector3 axis = Quaternion.AngleAxis(20f, bomb.up) * bomb.forward;
        if (quadrant > 0)
        {
            axis = Quaternion.AngleAxis(180f, bomb.forward) * axis;
        }
        
        Vector3 cross = Vector3.Cross(axis, clickVector);
        float dir = Vector3.Dot(cross.normalized, (Camera.main.transform.position - bomb.position).normalized);
        Debug.Log(dir);
        Debug.DrawRay( bomb.position, cross, Color.red, 5f);
        
        float angle = 5f;
        if (dir > 0)
        {
            angle *= -1f;
        }

        var qua = Quaternion.AngleAxis(angle, axis);
        var restore = bomb.rotation;
        bomb.DORotate((qua * bomb.rotation).eulerAngles, 0.1f).SetEase(Ease.OutBack).onComplete += () =>
        {
            bomb.DORotate(restore.eulerAngles, 0.4f).SetEase(Ease.OutBack);
        };
        #endregion
    }
}
