using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using UnityEngine.UI;

// Abort / Detonate / Hold
public class Button : MonoBehaviour
{
    public string[] labels = {"Abort", "Detonate", "Hold"};
    private Outlinable outline;

    private void Start()
    {
        outline = GetComponent<Outlinable>();
        outline.enabled = false;
        
        
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }
}
