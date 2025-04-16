using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using UnityEngine.UI;

public class KeypadButton : MonoBehaviour
{
    public event Action<KeypadButton> OnClick;

    public Renderer led;

    [SerializeField]private Outlinable outline;

    private void Start()
    {
        outline.enabled = false;
    }

    public void BlinkLed(Color color)
    {
        StartCoroutine(BlinkLed_Coroutine(color));
    }

    private IEnumerator BlinkLed_Coroutine(Color color)
    {
        WaitForSeconds wait = new WaitForSeconds(0.4f);
        for (int i = 0; i < 3; i++)
        {
            led.material.SetColor("_EmissionColor", color);
            yield return wait;
            led.material.SetColor("_EmissionColor", Color.black);
            yield return wait;
        }
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseDown()
    {
        OnClick?.Invoke(this);
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }

    private void OnDisable()
    {
        led.material.SetColor("_EmissionColor", Color.green);
    }
}
