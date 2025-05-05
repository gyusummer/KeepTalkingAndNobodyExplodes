using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimonButton : ModulePart
{
    public GameObject highlight;
    public IEnumerator FlashHighlight(float duration)
    {
        highlight.SetActive(true);
        yield return new WaitForSeconds(duration);
        highlight.SetActive(false);
    }
    protected override void OnPointerDown()
    {
        StartCoroutine(FlashHighlight(0.3f));
        MainEvent?.Invoke(new PartEventInfo(this));
    }
}
