using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// A wire module can have 3-6 wires on it.
// Only the one correct wire needs to be cut to
// disarm the module.
// Wire ordering begins with the first on the top.

// BBRWY
public class WireModule : DisarmableModule
{
    public static readonly Color[] COLOR_LIST = {Color.black, Color.blue, Color.red, Color.white, Color.yellow};
    
    [SerializeField]private Wire[] wholeWire;
    
    private int activeWireCount;
    private Wire[] activeWires;
    private Color[] activeColors;
    
    private Wire keyWire;

    private void Start()
    {
        EssentialInit();
        Initialize();
    }

    public void Initialize()
    {
        for (int i = 0; i < wholeWire.Length; i++)
        {
            wholeWire[i].gameObject.SetActive(false);
        }
        
        activeWireCount = Random.Range(3, 7);
        activeWires = RandomUtil.GetRandomCombination(wholeWire,activeWireCount);
        activeColors = new Color[activeWires.Length];
        for (int i = 0; i < activeWireCount; i++)
        {
            activeWires[i].gameObject.SetActive(true);
            activeColors[i] = activeWires[i].color;
            activeWires[i].OnSnip += Judge;
        }
        
        SetKeyWire();
    }

    private void Judge(Wire wire)
    {
        if (wire == keyWire)
        {
            Disarm();
        }
        else
        {
            statusLED.LightRed();
            Bomb.Instance.Strike();
        }
    }

    private void SetKeyWire()
    {
        switch (activeWireCount)
        {
            case 3:
                // If there are no red wires, cut the second wire.
                // Otherwise, if the last wire is white, cut the last wire.
                // Otherwise, if there is more than one blue wire, cut the last blue wire.
                // Otherwise, cut the last wire.
                if (activeColors.Contains(Color.red) == false)
                {
                    keyWire = activeWires[1];
                }
                else if (activeColors.Last() == Color.white)
                {
                    keyWire = activeWires.Last();
                }
                else if (activeColors.Count(c => c == Color.blue) > 1)
                {
                    int keyIndex = Array.FindIndex(activeColors, color => color == Color.blue);
                    keyWire = activeWires[keyIndex];
                }
                else
                {
                    keyWire = activeWires.Last();
                }
                break;
            case 4:
                // If there is more than one red wire and the last digit of the serial number is odd, cut the last red wire.
                // Otherwise, if the last wire is yellow and there are no red wires, cut the first wire.
                // Otherwise, if there is exactly one blue wire, cut the first wire.
                // Otherwise, if there is more than one yellow wire, cut the last wire.
                // Otherwise, cut the second wire.
                if (activeColors.Count(c => c == Color.red) > 1 && Bomb.Instance.IsSerialOdd())
                {
                    int keyIndex = Array.FindIndex(activeColors, color => color == Color.red);
                    keyWire = activeWires[keyIndex];
                }
                else if (activeColors.Last() == Color.yellow && activeColors.Contains(Color.red) == false)
                {
                    keyWire = activeWires[0];
                }
                else if (activeColors.Count(c => c == Color.blue) == 1)
                {
                    keyWire = activeWires[0];
                }
                else if (activeColors.Count(c => c == Color.yellow) > 1)
                {
                    keyWire = activeWires.Last();
                }
                else
                {
                    keyWire = activeWires[1];
                }
                break;
            case 5:
                // If the last wire is black and the last digit of the serial number is odd, cut the fourth wire.
                if (activeColors.Last() == Color.black && Bomb.Instance.IsSerialOdd())
                {
                    keyWire = activeWires[3]; // 4번째 와이어
                }
                // Otherwise, if there is exactly one red wire and there is more than one yellow wire, cut the first wire.
                else if (activeColors.Count(c => c == Color.red) == 1 && activeColors.Count(c => c == Color.yellow) > 1)
                {
                    keyWire = activeWires[0]; // 1번째 와이어
                }
                // Otherwise, if there are no black wires, cut the second wire.
                else if (activeColors.Contains(Color.black) == false)
                {
                    keyWire = activeWires[1]; // 2번째 와이어
                }
                // Otherwise, cut the first wire.
                else
                {
                    keyWire = activeWires[0]; // 1번째 와이어
                }
                break;
            case 6:
                // If there are no yellow wires and the last digit of the serial number is odd, cut the third wire.
                if (!activeColors.Contains(Color.yellow) && Bomb.Instance.IsSerialOdd())
                {
                    keyWire = activeWires[2]; // 세 번째 와이어 (인덱스 2)
                }
                // Otherwise, if there is exactly one yellow wire and there is more than one white wire, cut the fourth wire.
                else if (activeColors.Count(c => c == Color.yellow) == 1 && activeColors.Count(c => c == Color.white) > 1)
                {
                    keyWire = activeWires[3]; // 네 번째 와이어 (인덱스 3)
                }
                // Otherwise, if there are no red wires, cut the last wire.
                else if (!activeColors.Contains(Color.red))
                {
                    keyWire = activeWires.Last(); // 마지막 와이어 (.Last())
                }
                // Otherwise, cut the fourth wire.
                else
                {
                    keyWire = activeWires[3]; // 네 번째 와이어 (인덱스 3)
                }
                break;
        }
    }

    protected override void Disarm()
    {
        statusLED.LightGreen();
        for (int i = 0; i < activeWires.Length; i++)
        {
            activeWires[i].enabled = false;
        }
        this.enabled = false;
        Debug.Log("WireModule Disarmed");
    }

    private void OnDestroy()
    {
        for (int i = 0; i < activeWireCount; i++)
        {
            activeWires[i].OnSnip -= Judge;
        }
    }
}
