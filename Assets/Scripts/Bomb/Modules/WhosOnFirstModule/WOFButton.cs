using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WOFButton : ModulePart, IPointerClickHandler
{
    private TMP_Text text;
    private Collider collider;

    protected override void Init()
    {
        collider = GetComponentInChildren<Collider>();
        text = GetComponentInChildren<TMP_Text>();
    }

    public void SetLabel(string label)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Pause();
        sequence.AppendCallback(() =>
        {
            collider.enabled = false;
        });
        sequence.Append(transform.DOLocalMoveZ(transform.localPosition.z + 0.02f, 0.5f));
        sequence.AppendCallback(() =>
        {
            text.text = label;
        });
        sequence.Append(transform.DOLocalMoveZ(transform.localPosition.z, 0.5f));
        sequence.AppendCallback(() =>
        {
            collider.enabled = true;
        });
        sequence.Restart();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MainEvent?.Invoke(new PartEventInfo(this));
    }
}