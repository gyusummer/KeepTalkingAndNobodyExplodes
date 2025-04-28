using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using EPOOutline;
using UnityEngine;
using Random = UnityEngine.Random;

// must be white wire
// other color wires don't matter if you change the color;
public class TutorialWire : ModulePart
{
    [SerializeField]private Renderer intact;
    [SerializeField]private Renderer[] snipped;
    
    protected override void Init()
    {
        buttonDown = Resources.Load<AudioClip>(StaticStrings.AudioClipPath.WireSnip) as AudioClip;
        
        for (int i = 0; i < snipped.Length; i++)
        {
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

        if (intact.material.color == Color.blue)
        {
            ControlTutorialManager.Instance.BombEvent("BlueWireCut");
        }
    }
    protected override void OnButtonDown()
    {
        SnipWire();
    }
}