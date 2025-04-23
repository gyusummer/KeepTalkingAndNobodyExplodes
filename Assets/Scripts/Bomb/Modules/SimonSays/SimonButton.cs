using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimonButton : ModulePart, IPointerClickHandler
{
    public GameObject highlight;
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(FlashHighlight(0.3f));
        MainEvent?.Invoke(new PartEventInfo(this));
    }
    public IEnumerator FlashHighlight(float duration)
    {
        highlight.SetActive(true);
        yield return new WaitForSeconds(duration);
        highlight.SetActive(false);
    }
}
