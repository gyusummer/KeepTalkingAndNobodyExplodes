using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeypadModule : DisarmableModule
{
    [SerializeField] private KeypadButton[] keypadButtons;
    [SerializeField] private KeypadModuleSymbolGroup[] symbolGroup;
    private Texture2D[] symbols;

    private int nextButtonCursor = 0;

    protected override void Init()
    {
        var selectedGroup = symbolGroup[Random.Range(0, symbolGroup.Length)];
        symbols = RandomUtil.GetSortedRandomSubset(selectedGroup.symbols, 4);
        InitializeSymbols();
    }

    private void InitializeSymbols()
    {
        keypadButtons = RandomUtil.GetShuffled(keypadButtons);
        for (int i = 0; i < symbols.Length; i++)
        {
            keypadButtons[i].symbolImage.material.mainTexture = symbols[i];
        }
    }
    protected override void SetKeyEvent()
    {
        keyEvent.part = keypadButtons[nextButtonCursor];
    }
    protected override void Hit(PartEventInfo info)
    {
        info.part.MainEvent -= Judge;
        info.part.enabled = false;
        info.part.GetComponentInChildren<Collider>().enabled = false;
        nextButtonCursor++;
        if (nextButtonCursor == keypadButtons.Length)
        {
            Disarm();
            return;
        }
        SetKeyEvent();
    }
    protected override void Strike(PartEventInfo info)
    {
        var btn = info.part as KeypadButton;
        btn.BlinkLed(Color.red);
        base.Strike(info);
    }
}
