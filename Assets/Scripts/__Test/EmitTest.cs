using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitTest : MonoBehaviour
{
    public Material mat;
    public bool isEmitting = false;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            mat.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                mat.SetColor("_EmissionColor", Color.red);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                mat.SetColor("_EmissionColor", Color.blue);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                mat.SetColor("_EmissionColor", Color.white);
            }
        }
    }
}