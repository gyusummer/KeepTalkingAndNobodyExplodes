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
        part = null;
        time = 0;
        parameter = string.Empty;
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
        outline = GetComponent<Outlinable>();
        outline.OutlineParameters.Color = Color.red;
        outline.enabled = false;
        Init();
    }

    protected virtual void Init() {}

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
