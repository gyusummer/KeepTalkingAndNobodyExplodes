using System;
using System.Collections;
using System.Collections.Generic;
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
        
    }

    public PartEventInfo(ModulePart part)
    {
        this.part = part;
    }
}
public abstract class ModulePart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action<PartEventInfo> MainEvent;
    public Action SubEvent;
    
    public Outlinable outline;

    private void Start()
    {
        outline = GetComponentInChildren<Outlinable>();
        outline.enabled = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(this.enabled == false) return;
        outline.enabled = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(this.enabled == false) return;
        outline.enabled = false;
    }
    protected virtual void OnDisable()
    {
        if (outline is not null)
        {
            outline.enabled = false;
        }
    }
}
