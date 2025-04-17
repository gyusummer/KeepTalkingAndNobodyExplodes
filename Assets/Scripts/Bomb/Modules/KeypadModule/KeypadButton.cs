using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using UnityEngine.UI;

public class KeypadButton : MonoBehaviour
{
    private static readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");
    
    public event Action<KeypadButton> OnClick;

    public Renderer led;
    public Collider collider;

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
            led.material.SetColor(EMISSION_COLOR, color);
            yield return wait;
            led.material.SetColor(EMISSION_COLOR, Color.black);
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
        led.material.SetColor(EMISSION_COLOR, Color.green);
    }
}
