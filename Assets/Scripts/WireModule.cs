using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

// A wire module can have 3-6 wires on it.
// Only the one correct wire needs to be cut to
// disarm the module.
// Wire ordering begins with the first on the top.

// BBRWY
public class WireModule : MonoBehaviour
{
    private readonly Color[] colors = {Color.black, Color.blue, Color.red, Color.white, Color.yellow};
    
    [SerializeField]private Wire[] wires;

    private int wireCount;
    public void Initialize()
    {
        wireCount = Random.Range(3, 7);
        

        for (int i = 0; i < wireCount; i++)
        {
            wires[i].mat.color = colors[Random.Range(0, colors.Length)];
        }
    }
}
