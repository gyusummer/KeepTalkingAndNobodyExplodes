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
    private static readonly string[] LABEL_LIST = {"Abort", "Detonate", "Hold"};
    private static readonly int OPEN = Animator.StringToHash("Open");
    private static readonly int CLOSE = Animator.StringToHash("Close");
    private static readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");

    [SerializeField]private Animator animator;
    [SerializeField]private TMP_Text text;
    [SerializeField]private Renderer led;

    private Button button;
    private string label;
    private Color buttonColor;
    private Color stripColor;

    private Coroutine ledCoroutine = null;
    
    protected override void Init()
    {
        buttonColor = COLOR_LIST[UnityEngine.Random.Range(0, COLOR_LIST.Length)];
        stripColor = COLOR_LIST[UnityEngine.Random.Range(0, COLOR_LIST.Length)];
        
        label = LABEL_LIST[UnityEngine.Random.Range(0, LABEL_LIST.Length)];
        text.text = label;
        text.alpha = 0.5f;

        button = GetComponentInChildren<Button>();
        button.GetComponent<Renderer>().material.color = buttonColor;
        button.SubEvent += TurnOnLed;
    }
    protected override bool CompareKeyEvent(PartEventInfo partEvent)
    {
        string timer = bomb.timerModule.leftTimeString;
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
        int batteryCount = bomb.battery.Length;
        if (buttonColor == Color.blue && label == "Abort")
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (batteryCount > 1 && label == "Detonate")
        {
            keyEvent.parameter = "-1";
        }
        else if (buttonColor == Color.white && bomb.indicator == "CAR")
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (batteryCount > 2 && bomb.indicator == "FRK")
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
    protected override void Judge(PartEventInfo partEvent)
    {
        TurnOffLed();
        base.Judge(partEvent);
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
    private void TurnOnLed()
    {
        if (ledCoroutine == null)
        {
            ledCoroutine = StartCoroutine(LED_Coroutine());
            button.isLedOn = true;
        }
    }
    private void TurnOffLed()
    {
        if (ledCoroutine != null)
        {
            StopCoroutine(ledCoroutine);
            ledCoroutine = null;
            led.material.SetColor(EMISSION_COLOR, Color.black);
            button.isLedOn = false;
        }
    }
    private IEnumerator LED_Coroutine()
    {
        float intensity = 0;
        bool isRising = true;
        float weight = 3;
        while(led.gameObject.activeInHierarchy)
        {
            if(isRising)
            {
                while(intensity < 2f)
                {
                    intensity += weight * Time.deltaTime;
                    led.material.SetColor(EMISSION_COLOR, stripColor * intensity);
                    yield return null;
                }
                isRising = false;
            }
            else
            {
                while(intensity > -1f)
                {
                    intensity -= weight * Time.deltaTime;
                    led.material.SetColor(EMISSION_COLOR, stripColor * intensity);
                    yield return null;
                }
                isRising = true;
            }
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
    protected override void OnDestroy()
    {
        button.SubEvent -= TurnOnLed;
        button.MainEvent -= Judge;
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
