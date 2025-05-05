using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using EPOOutline;
using UnityEngine;
using Random = UnityEngine.Random;

// must be white wire
// other color wires don't matter if you change the color;
public class Wire : ModulePart
{
    [HideInInspector]public Color color;

    [SerializeField]private Renderer intact;
    [SerializeField]private Renderer[] snipped;
    
    protected override void Init()
    {
        buttonDown = Resources.Load<AudioClip>(StaticStrings.AudioClipPath.WireSnip);
        
        intact.material.color = color;
        for (int i = 0; i < snipped.Length; i++)
        {
            snipped[i].material.color = color;
            snipped[i].gameObject.SetActive(false);
        }
    }

    private void SnipWire()
    {
        intact.enabled = false;
        for (int i = 0; i < snipped.Length; i++)
        {
            snipped[i].gameObject.SetActive(true);
        }
        MainEvent?.Invoke(new PartEventInfo(this));
    }
    protected override void OnPointerDown()
    {
        SnipWire();
    }
}
