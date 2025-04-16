using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonModule : DisarmableModule, ISelectable
{
    private static readonly Color[] COLOR_LIST = {Color.blue, Color.red, Color.white, Color.yellow};
    private static readonly string[] LABEL_LIST = {"Abort", "Detonate", "Hold"};
    
    private static readonly int OPEN = Animator.StringToHash("Open");
    private static readonly int CLOSE = Animator.StringToHash("Close");
    [SerializeField]private Animator animator;
    [SerializeField]private TMP_Text text;

    private Button button;
    private string label;
    private Color buttonColor;
    private Color stripColor;

    private string key;
    
    private void Start()
    {
        buttonColor = COLOR_LIST[UnityEngine.Random.Range(0, COLOR_LIST.Length)];
        stripColor = COLOR_LIST[UnityEngine.Random.Range(0, COLOR_LIST.Length)];
        
        label = LABEL_LIST[UnityEngine.Random.Range(0, LABEL_LIST.Length)];
        text.text = label;

        button = GetComponentInChildren<Button>();
        button.GetComponent<Renderer>().material.color = buttonColor;
        button.OnClick += OnButtonClick;
        
        SetKey();
    }

    private void OnButtonClick(float holdTime)
    {
        Judge(holdTime < 0.1f);
    }

    private void Judge(bool isImmediateRelease)
    {
        string timer = Bomb.TimerModule.leftTimeString;
        bool isTimerHasKey = timer.Contains(key);
        if (key == "0" && isImmediateRelease)
        {
            Disarm();
        }
        else if(isImmediateRelease == false && isTimerHasKey)
        {
            Disarm();
        }
        else
        {
            Bomb.Instance.Strike();
        }
    }

    private void SetKey()
    {
        int batteryCount = Bomb.Battery.Length;
        if (buttonColor == Color.blue && label == "Abort")
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (batteryCount > 1 && label == "Detonate")
        {
            key = "0";
        }
        else if (buttonColor == Color.white && Bomb.Indicator == "CAR")
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (batteryCount > 2 && Bomb.Indicator == "FRK")
        {
            key = "0";
        }
        else if (buttonColor == Color.yellow)
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (buttonColor == Color.red && label == "Hold")
        {
            key = "0";
        }
        else
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
    }

    private void DelayedRelease()
    {
        if (stripColor == Color.blue)
        {
            key = "4";
        }
        else if (stripColor == Color.yellow)
        {
            key = "5";
        }
        else
        {
            key = "1";
        }
    }
    protected override void Disarm()
    {
        button.enabled = false;
        this.enabled = false;
        Debug.Log("ButtonModule Disarmed");
    }
    private void OpenLid()
    {
        animator.ResetTrigger(CLOSE);
        animator.SetTrigger(OPEN);
    }
    private void CloseLid()
    {
        animator.ResetTrigger(OPEN);
        animator.SetTrigger(CLOSE);
    }

    public void OnSelected()
    {
        OpenLid();
    }

    public void OnDeselected()
    {
        CloseLid();
    }

    private void OnDestroy()
    {
        button.OnClick -= OnButtonClick;
    }
}
