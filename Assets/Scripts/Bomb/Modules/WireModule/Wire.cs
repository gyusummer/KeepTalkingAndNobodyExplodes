using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using Random = UnityEngine.Random;

// must be white wire
// other color wires don't matter if you change the color;
public class Wire : MonoBehaviour
{
    public event Action<Wire> OnSnip;
    
    [HideInInspector]public Color color;

    private Outlinable outline;
    [SerializeField]private Renderer intact;
    [SerializeField]private Renderer[] snipped;
    private void Start()
    {
        outline = GetComponent<Outlinable>();
        outline.enabled = false;
        
        color = WireModule.COLOR_LIST[Random.Range(0, WireModule.COLOR_LIST.Length)];
        
        intact.material.color = color;
        for (int i = 0; i < snipped.Length; i++)
        {
            snipped[i].material.color = color;
            snipped[i].gameObject.SetActive(false);
        }
    }

    public void SnipWire()
    {
        // play sound
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

    private void OnMouseEnter()
    {
        if(this.enabled == false) return;
        outline.enabled = true;
        Debug.Log("Mouse Enter");
    }

    private void OnMouseExit()
    {
        if(this.enabled == false) return;
        outline.enabled = false;
        Debug.Log("Mouse Exit");
    }
}
