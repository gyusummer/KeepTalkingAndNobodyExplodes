using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysModule : DisarmableModule
{
    public SimonButton[] buttons;
    
    protected override void Init()
    {
        buttons = parts as SimonButton[];
    }

    protected override void SetKeyEvent()
    {
        
    }
}
