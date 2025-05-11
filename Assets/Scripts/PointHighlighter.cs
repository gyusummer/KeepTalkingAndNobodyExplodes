using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outlinable))]
public class PointHighlighter : MonoBehaviour
{
    private Outlinable outline;
    private AudioClip outlineTick;

    private void Awake()
    {
	    outlineTick = Resources.Load(StaticStrings.AudioClipPath.Tick) as AudioClip;
	    outline = GetComponent<Outlinable>();
	    
        outline.enabled = false;
    }

    public void SetOutlineColor(Color color)
    {
	    outline.OutlineParameters.Color = color;
    }

    public void OnMouseEnter()
    {
		AudioManager.Instance.PlaySfx(outlineTick);
		outline.enabled = true;
	}

    public void OnMouseExit()
    {
		outline.enabled = false;
	}

	private void OnDisable()
	{
		outline.enabled = false;
	}
}
