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

    private void Start()
    {
        EssentialInit();
        
        var selectedGroup = symbolGroup[Random.Range(0, symbolGroup.Length)];
        symbols = RandomUtil.GetSortedRandomSubset(selectedGroup.symbols, 4);
        InitializeSymbols();

        for (int i = 0; i < keypadButtons.Length; i++)
        {
            keypadButtons[i].OnClick += Judge;
        }
    }

    private void InitializeSymbols()
    {
        keypadButtons = RandomUtil.GetShuffled(keypadButtons);
        for (int i = 0; i < symbols.Length; i++)
        {
            keypadButtons[i].symbolImage.material.mainTexture = symbols[i];
        }
    }

    private void Judge(KeypadButton button)
    {
        if (keypadButtons[nextButtonCursor] == button)
        {
            button.OnClick -= Judge;
            button.enabled = false;
            button.collider.enabled = false;
            nextButtonCursor++;
        }
        else
        {
            button.BlinkLed(Color.red);
            statusLED.LightRed();
            bomb.Strike(this);
        }

        if (nextButtonCursor == keypadButtons.Length)
        {
            Disarm();
        }

        Debug.Log(nextButtonCursor);
    }
    protected override void Disarm()
    {
        statusLED.LightGreen();
        this.enabled = false;
        Debug.Log("KeypadModule Disarmed");
        bomb.CurDisarm++;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < keypadButtons.Length; i++)
        {
            keypadButtons[i].OnClick -= Judge;
        }
    }
}
