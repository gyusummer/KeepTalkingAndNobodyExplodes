using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeypadModule : DisarmableModule
{
    [SerializeField] private KeypadButton[] keypadButtons;
    [SerializeField] private Renderer[] symbolRenderers;
    [SerializeField] private KeypadModuleSymbolGroup[] symbolGroup;
    private Texture2D[] symbols;

    private int nextButtonCursor = 0;

    private void Start()
    {
        var selectedGroup = symbolGroup[Random.Range(0, symbolGroup.Length)];
        symbols = RandomUtil.GetRandomCombination(selectedGroup.symbols, 4);
        InitalizeSymbols();

        for (int i = 0; i < keypadButtons.Length; i++)
        {
            keypadButtons[i].OnClick += Judge;
        }
    }

    private void InitalizeSymbols()
    {
        Texture2D[] shuffledSymbols = (Texture2D[])symbols.Clone();
        for (int i = shuffledSymbols.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffledSymbols[i], shuffledSymbols[j]) = (shuffledSymbols[j], shuffledSymbols[i]);
        }
        for (int i = 0; i < shuffledSymbols.Length; i++)
        {
            symbolRenderers[i].material.mainTexture = shuffledSymbols[i];
        }
    }

    private void Judge(KeypadButton button)
    {
        if (keypadButtons[nextButtonCursor] == button)
        {
            button.OnClick -= Judge;
            button.enabled = false;
            nextButtonCursor++;
        }
        else
        {
            button.BlinkLed(Color.red);
            Bomb.Instance.Strike();
        }

        if (nextButtonCursor == keypadButtons.Length)
        {
            Disarm();
        }

        Debug.Log(nextButtonCursor);
    }
    protected override void Disarm()
    {
        this.enabled = false;
        Debug.Log("KeypadModule Disarmed");
    }

    private void OnDestroy()
    {
        for (int i = 0; i < keypadButtons.Length; i++)
        {
            keypadButtons[i].OnClick -= Judge;
        }
    }
}
