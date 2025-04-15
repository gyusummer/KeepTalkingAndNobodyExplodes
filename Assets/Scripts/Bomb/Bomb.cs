using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance;
    public static string Serial;
    private int opportunity = 3;
    public TimeSpan LimitTime = new TimeSpan(0,5,0);

    private void Awake()
    {
        Instance = this;
        // TODO: Randomize serial
        Serial = "4X8SB2";
        
    }

    public void Strike()
    {
        Debug.Log("Strike");
        opportunity--;
        if (opportunity <= 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Debug.Log("Explode");
    }

    public bool IsSerialOdd()
    {
        char serialLast = Serial.Last();
        int lastAsInt = serialLast - 48;
        return lastAsInt % 2 == 1;
    }
}
