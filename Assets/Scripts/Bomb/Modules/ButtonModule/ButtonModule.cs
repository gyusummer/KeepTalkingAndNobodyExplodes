using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class ButtonModule : DisarmableModule
{
    private static readonly Color[] COLOR_LIST = {Color.blue, Color.red, Color.white, Color.yellow};
    private static readonly string[] LABEL_LIST = {"Abort", "Detonate", "Hold",};
    private static readonly int OPEN = Animator.StringToHash("Open");
    private static readonly int CLOSE = Animator.StringToHash("Close");

    [SerializeField]private Animator animator;
    [SerializeField]private TMP_Text text;
    [SerializeField]private Renderer led;

    private Button button;
    private string label;
    private Color buttonColor;
    private Color stripColor;
    
    protected override void Init()
    {
        buttonColor = COLOR_LIST[UnityEngine.Random.Range(0, COLOR_LIST.Length)];
        stripColor = COLOR_LIST[UnityEngine.Random.Range(0, COLOR_LIST.Length)];
        label = RandomUtil.GetRandomSubset(LABEL_LIST, 1)[0];
        
        text.text = label;
        text.alpha = 0.5f;

        button = GetComponentInChildren<Button>();
        button.GetComponent<Renderer>().material.color = buttonColor;
        button.StripColor = stripColor;
    }
    protected override bool IsCorrectEvent(PartEventInfo partEvent)
    {
        string timer = Bomb.TimerModule.LeftTimeString;
        bool isTimerHasKey = timer.Contains(keyEvent.parameter);
        bool isImmediateRelease = partEvent.time <= 0.2f;
        if (keyEvent.parameter == "-1" && isImmediateRelease)
        {
            return true;
        }
        else if (isTimerHasKey)
        {
            return true;
        }
        return false;
    }
    protected override void SetKeyEvent()
    {
        int batteryCount = Bomb.Battery.Length;
        if (buttonColor == Color.blue && label == "Abort")
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (batteryCount > 1 && label == "Detonate")
        {
            keyEvent.parameter = "-1";
        }
        else if (buttonColor == Color.white && Bomb.Indicator == "CAR")
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (batteryCount > 2 && Bomb.Indicator == "FRK")
        {
            keyEvent.parameter = "-1";
        }
        else if (buttonColor == Color.yellow)
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (buttonColor == Color.red && label == "Hold")
        {
            keyEvent.parameter = "-1";
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
            keyEvent.parameter = "4";
        }
        else if (stripColor == Color.yellow)
        {
            keyEvent.parameter = "5";
        }
        else
        {
            keyEvent.parameter = "1";
        }
    }
    public override ISelectable OnSelected(Transform selectPosition)
    {
        OpenLid();
        return base.OnSelected(selectPosition);
    }
    public override ISelectable OnDeselected()
    {
        CloseLid();
        return base.OnDeselected();
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
}
