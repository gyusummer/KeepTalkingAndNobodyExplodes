using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using UnityEngine.UI;

public class KeypadButton : ModulePart
{
    private static readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");

    public Renderer symbolImage;
    public Renderer led;

    private Coroutine ledCoroutine = null;

    public void BlinkLed(Color color)
    {
        if (ledCoroutine != null)
        {
            StopCoroutine(ledCoroutine);
        }
        ledCoroutine = StartCoroutine(BlinkLed_Coroutine(color));
    }

    private IEnumerator BlinkLed_Coroutine(Color color)
    {
        WaitForSeconds wait = new WaitForSeconds(0.4f);
        for (int i = 0; i < 3; i++)
        {
            led.material.SetColor(EMISSION_COLOR, color);
            yield return wait;
            led.material.SetColor(EMISSION_COLOR, Color.black);
            yield return wait;
        }
    }

    private void OnMouseDown()
    {
        MainEvent?.Invoke(new PartEventInfo(this));
    }

    protected override void OnDisable()
    {
        if (outline is not null)
        {
            outline.enabled = false;
        }
        if (ledCoroutine != null)
        {
            StopCoroutine(ledCoroutine);
            ledCoroutine = null;
        }
        
        led.material.SetColor(EMISSION_COLOR, Color.green);
    }
}
