using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using Random = UnityEngine.Random;

// must be white wire
// other color wires don't matter if you change the color;
public class Wire : ModulePart
{
    public event Action<Wire> OnSnip;
    
    [HideInInspector]public Color color;

    [SerializeField]private Renderer intact;
    [SerializeField]private Renderer[] snipped;
    private void Awake()
    {
        color = WireModule.COLOR_LIST[Random.Range(0, WireModule.COLOR_LIST.Length)];
        
        intact.material.color = color;
        for (int i = 0; i < snipped.Length; i++)
        {
            snipped[i].material.color = color;
            snipped[i].gameObject.SetActive(false);
        }
    }

    private void SnipWire()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < snipped.Length; i++)
        {
            snipped[i].gameObject.SetActive(true);
        }
        OnSnip?.Invoke(this);
    }
    private void OnMouseDown()
    {
        if(this.enabled == false) return;
        SnipWire();
    }
}
