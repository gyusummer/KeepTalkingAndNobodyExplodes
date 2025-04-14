using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// must be white wire
// other color wires dont matter if you change the color;
public class Wire : MonoBehaviour
{
    public Material mat;
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
        mat.color = Color.white;
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        // mat.color = Color.black;
    }
}
