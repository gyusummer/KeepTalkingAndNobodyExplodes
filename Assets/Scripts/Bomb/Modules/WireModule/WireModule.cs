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
    
    private int activeWireCount;
    private ModulePart[] activeWires;
    private Color[] activeColors;
    
    protected override void Init()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].gameObject.SetActive(false);
        }
        
        activeWireCount = Random.Range(3, 7);
        activeWires = RandomUtil.GetSortedRandomSubset(parts,activeWireCount);
        activeColors = new Color[activeWires.Length];
        for (int i = 0; i < activeWireCount; i++)
        {
            activeWires[i].gameObject.SetActive(true);
            activeColors[i] = COLOR_LIST[Random.Range(0, COLOR_LIST.Length)];
            (activeWires[i] as Wire).color = activeColors[i];
        }
    }
    protected override void SetKeyEvent()
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
                    keyEvent.part = activeWires[1];
                }
                else if (activeColors.Last() == Color.white)
                {
                    keyEvent.part = activeWires.Last();
                }
                else if (activeColors.Count(c => c == Color.blue) > 1)
                {
                    int keyIndex = Array.FindIndex(activeColors, color => color == Color.blue);
                    keyEvent.part = activeWires[keyIndex];
                }
                else
                {
                    keyEvent.part = activeWires.Last();
                }
                break;
            case 4:
                // If there is more than one red wire and the last digit of the serial number is odd, cut the last red wire.
                // Otherwise, if the last wire is yellow and there are no red wires, cut the first wire.
                // Otherwise, if there is exactly one blue wire, cut the first wire.
                // Otherwise, if there is more than one yellow wire, cut the last wire.
                // Otherwise, cut the second wire.
                if (activeColors.Count(c => c == Color.red) > 1 && Bomb.IsSerialOdd())
                {
                    for(int i = activeColors.Length - 1; i > 0; i--)
                    {
                        if (activeColors[i] == Color.red)
                        {
                            keyEvent.part = activeWires[i];
                            break;
                        }
                    }
                }
                else if (activeColors.Last() == Color.yellow && activeColors.Contains(Color.red) == false)
                {
                    keyEvent.part = activeWires[0];
                }
                else if (activeColors.Count(c => c == Color.blue) == 1)
                {
                    keyEvent.part = activeWires[0];
                }
                else if (activeColors.Count(c => c == Color.yellow) > 1)
                {
                    keyEvent.part = activeWires.Last();
                }
                else
                {
                    keyEvent.part = activeWires[1];
                }
                break;
            case 5:
                // If the last wire is black and the last digit of the serial number is odd, cut the fourth wire.
                if (activeColors.Last() == Color.black && Bomb.IsSerialOdd())
                {
                    keyEvent.part = activeWires[3]; // 4번째 와이어
                }
                // Otherwise, if there is exactly one red wire and there is more than one yellow wire, cut the first wire.
                else if (activeColors.Count(c => c == Color.red) == 1 && activeColors.Count(c => c == Color.yellow) > 1)
                {
                    keyEvent.part = activeWires[0]; // 1번째 와이어
                }
                // Otherwise, if there are no black wires, cut the second wire.
                else if (activeColors.Contains(Color.black) == false)
                {
                    keyEvent.part = activeWires[1]; // 2번째 와이어
                }
                // Otherwise, cut the first wire.
                else
                {
                    keyEvent.part = activeWires[0]; // 1번째 와이어
                }
                break;
            case 6:
                // If there are no yellow wires and the last digit of the serial number is odd, cut the third wire.
                if (!activeColors.Contains(Color.yellow) && Bomb.IsSerialOdd())
                {
                    keyEvent.part = activeWires[2]; // 세 번째 와이어 (인덱스 2)
                }
                // Otherwise, if there is exactly one yellow wire and there is more than one white wire, cut the fourth wire.
                else if (activeColors.Count(c => c == Color.yellow) == 1 && activeColors.Count(c => c == Color.white) > 1)
                {
                    keyEvent.part = activeWires[3]; // 네 번째 와이어 (인덱스 3)
                }
                // Otherwise, if there are no red wires, cut the last wire.
                else if (!activeColors.Contains(Color.red))
                {
                    keyEvent.part = activeWires.Last(); // 마지막 와이어 (.Last())
                }
                // Otherwise, cut the fourth wire.
                else
                {
                    keyEvent.part = activeWires[3]; // 네 번째 와이어 (인덱스 3)
                }
                break;
        }
    }
}
