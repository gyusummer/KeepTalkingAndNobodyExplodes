using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialButton : ModulePart, IPointerClickHandler
{
    private float holdTime;
    private bool isHolding;

    [SerializeField] private AudioClip buttonUp;
    private void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
        }

        if (holdTime >= 1f)
        {
            ReleaseButton();
        }
    }

    protected override void OnPointerDown()
    {
        if(this.enabled == false) return;
        holdTime = 0;
        isHolding = true;
        transform.Translate(0, -0.01f, 0);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (isHolding == false)
        {
            return;
        }
        ReleaseButton();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (holdTime > 0.5f)
        {
            ReleaseButton();
        }
    }
    private void ReleaseButton()
    {
        ControlTutorialManager.Instance.BombEvent("ButtonReleased");
        
        AudioManager.Instance.PlaySfx(buttonUp);
        isHolding = false;
        transform.Translate(0, 0.01f, 0);
        holdTime = 0;
    }
}