using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance;
    public static string Serial;
    public static string Indicator;
    public static string[] Battery;
    public static TimerModule TimerModule;

    private static readonly string[] INDICATOR_LIST =
        { "SND", "CLR", "CAR", "IND", "FRQ", "SIG", "NSA", "MSA", "TRN", "BOB", "FRK" };
    
    public TimeSpan LimitTime = new TimeSpan(0,5,0);
    private int opportunity = 3;
    private int strikeCount = 0;

    private void Awake()
    {
        Instance = this;
        // TODO: Randomize serial
        Serial = "4X8SB2";
        Indicator = INDICATOR_LIST[Random.Range(0, INDICATOR_LIST.Length)];
        
        Battery = new string[Random.Range(1, 5)];
        for (int i = 0; i < Battery.Length; i++)
        {
            Battery[i] = Random.Range(0, 1) < 0.5f ? "AA" : "D";
        }
    }

    private void Start()
    {
        TimerModule = GetComponentInChildren<TimerModule>();
        
        Debug.Log($"Serial: {Serial}\n" +
                  $"Indicator: {Indicator}\n" +
                  $"Battery: {Battery.Length}\n" +
                  $"LimitTime: {LimitTime}");
    }

    public void Strike()
    {
        Debug.Log("Strike");
        strikeCount++;
        
        if (strikeCount >= opportunity)
        {
            Explode();
        }
        else
        {
            TimerModule.strikeCounter[strikeCount - 1].SetActive(true);
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
