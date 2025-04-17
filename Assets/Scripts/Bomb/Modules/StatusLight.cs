using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusLight : MonoBehaviour
{
    private static readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");
    private Renderer render;

    private void Start()
    {
        render = GetComponent<Renderer>();
    }

    public void LightRed()
    {
        StopAllCoroutines();
        StartCoroutine(LightRed_Coroutine());
    }
    private IEnumerator LightRed_Coroutine()
    {
        render.material.SetColor(EMISSION_COLOR, Color.red);
        yield return new WaitForSeconds(2.0f);
        render.material.SetColor(EMISSION_COLOR, Color.black);
    }
    public void LightGreen()
    {
        StopAllCoroutines();
        render.material.SetColor(EMISSION_COLOR, Color.green);
    }
}
