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

    private string key;
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
        button.OnHold += TurnOnLed;
        button.OnButtonRelease += ButtonReleaseCallback;
        
        SetKey();
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

    private void ButtonReleaseCallback(float holdTime)
    {
        TurnOffLed();
        Judge(holdTime < 0.1f);
    }

    private void Judge(bool isImmediateRelease)
    {
        string timer = bomb.timerModule.leftTimeString;
        Debug.Log($"bomb {bomb}\n" +
                  $"timermodule {bomb.timerModule}\n" +
                  $"lefttime {timer}");
        bool isTimerHasKey = timer.Contains(key);
        if (key == "-1" && isImmediateRelease)
        {
            Disarm();
        }
        else if(isImmediateRelease == false && isTimerHasKey)
        {
            Disarm();
        }
        else
        {
            statusLED.LightRed();
            bomb.Strike(this);
        }
    }

    private void SetKey()
    {
        int batteryCount = bomb.battery.Length;
        if (buttonColor == Color.blue && label == "Abort")
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (batteryCount > 1 && label == "Detonate")
        {
            key = "-1";
        }
        else if (buttonColor == Color.white && bomb.indicator == "CAR")
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (batteryCount > 2 && bomb.indicator == "FRK")
        {
            key = "-1";
        }
        else if (buttonColor == Color.yellow)
        {
            DelayedRelease(); // Releasing a Held Button 참조
        }
        else if (buttonColor == Color.red && label == "Hold")
        {
            key = "-1";
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

    private void OnDestroy()
    {
        button.OnHold -= TurnOnLed;
        button.OnButtonRelease -= ButtonReleaseCallback;
    }
}
