using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public abstract class ModulePart : MonoBehaviour
{
    protected Outlinable outline;

    private void Start()
    {
        outline = GetComponentInChildren<Outlinable>();
        outline.enabled = false;
    }

    protected virtual void OnMouseEnter()
    {
        if(this.enabled == false) return;
        outline.enabled = true;
    }

    protected virtual void OnMouseExit()
    {
        if(this.enabled == false) return;
        outline.enabled = false;
    }
}
