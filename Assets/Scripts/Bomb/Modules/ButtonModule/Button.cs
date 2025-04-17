using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using TMPro;
using UnityEngine;


// 1. If the button is blue and the button says "Abort", hold the button and refer to "Releasing a Held Button".
// 2. If there is more than 1 battery on the bomb and the button says "Detonate", press and immediately release the button.
// 3. If the button is white and there is a lit indicator with label CAR, hold the button and refer to "Releasing a Held Button".
// 4. If there are more than 2 batteries on the bomb and there is a lit indicator with label FRK, press and immediately release the button.
// 5. If the button is yellow, hold the button and refer to "Releasing a Held Button".
// 6. If the button is red and the button says "Hold", press and immediately release the button.
// 7. If none of the above apply, hold the button and refer to "Releasing a Held Button".

// Releasing a Held Button
// Blue strip: release when the countdown timer has a 4 in any position.
// White strip: release when the countdown timer has a 1 in any position.
// Yellow strip: release when the countdown timer has a 5 in any position.
// Any other color strip: release when the countdown timer has a 1 in any position.

// Abort / Detonate / Hold

public class Button : MonoBehaviour
{
    public event Action OnHold;
    public event Action<float> OnButtonRelease;
    
    public bool isLedOn;
    
    private Outlinable outline;
    private float holdTime;
    private bool isHolding;

    private void Start()
    {
        outline = GetComponent<Outlinable>();
        outline.enabled = false;
    }

    private void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
            if (holdTime > 0.5f && isLedOn == false)
            {
                OnHold?.Invoke();
            }
        }
    }

    private void OnMouseDown()
    {
        if(this.enabled == false) return;
        holdTime = 0;
        isHolding = true;
    }

    private void OnMouseUpAsButton()
    {
        if(this.enabled == false) return;
        ReleaseButton();
    }

    private void OnMouseEnter()
    {
        if(this.enabled == false) return;
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        if(this.enabled == false) return;
        if (holdTime > 0.5f)
        {
            ReleaseButton();
        }
        outline.enabled = false;
    }
    private void ReleaseButton()
    {
        holdTime = 0;
        isHolding = false;
        OnButtonRelease?.Invoke(holdTime);
    }
}
