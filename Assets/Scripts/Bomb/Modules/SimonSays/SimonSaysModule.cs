using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysModule : DisarmableModule
{
    public SimonButton[] buttons;

    private static int[,] vowelKeyTable = new int[3, 4]
    {
        { 1, 0, 3, 2 }, // Blue, Red, Yellow, Green
        { 3, 2, 1, 0 }, // Yellow, Green, Blue, Red
        { 2, 0, 3, 1 } // Green, Red, Yellow, Blue
    };
    private static int[,] nVowelKeyTable = new int[3, 4]
    {
        { 1, 3, 2, 0 }, // Blue, Yellow, Green, Red
        { 0, 1, 3, 2 }, // Red, Blue, Yellow, Green
        { 3, 2, 1, 0 } // Yellow, Green, Blue, Red
    };
    private int[,] keyTable;
    private SimonButton[] flashOrder;
    private int keyCursor = 0; // Current button index
    private int flashCursor = 0; // Max Index of current flash routine
    private float flashDuration = 0.5f;
    private float waitDuration = 5f;

    protected override void Init()
    {
        buttons = new SimonButton[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            buttons[i] = parts[i] as SimonButton;
        }

        flashOrder = RandomUtil.GetShuffled(buttons);
        
        if (Bomb.HasSerialVowel())
            keyTable = vowelKeyTable;
        else
            keyTable = nVowelKeyTable;

        Bomb.OnBombStrike += SetKeyEvent;

        StartCoroutine(FlashRoutine_Coroutine());
    }
    protected override void SetKeyEvent()
    {
        int tableIndex = Array.IndexOf(buttons, flashOrder[keyCursor]);
        int keyIndex = keyTable[Bomb.CurStrike, tableIndex];
        keyEvent.part = parts[keyIndex];
    }

    private IEnumerator FlashRoutine_Coroutine()
    {
        while (this.enabled)
        {
            for (int i = 0; i < flashCursor + 1; i++)
            {
                yield return flashOrder[i].FlashHighlight(flashDuration);
            }
            yield return new WaitForSeconds(waitDuration);
        }
    }

    protected override void Hit(PartEventInfo info)
    {
        keyCursor++;
        if (keyCursor >= parts.Length)
        {
            Disarm();
            return;
        }
        if (keyCursor > flashCursor)
        {
            flashCursor++;
            ResetKeyCursor();
        }
        SetKeyEvent();
    }

    protected override void Strike(PartEventInfo info)
    {
        ResetKeyCursor();
        base.Strike(info);
    }

    private void ResetKeyCursor()
    {
        keyCursor = 0;
    }
}
