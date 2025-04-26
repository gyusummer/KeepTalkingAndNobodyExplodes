using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysModule : DisarmableModule
{
    public SimonButton[] buttons;

    private SimonButton[,] vowelKeyTable;
    private SimonButton[,] nVowelKeyTable;
    private SimonButton[] flashOrder;
    private int keyCursor = 0;
    private int flashCursor = 0;
    private float flashDuration = 0.5f;
    private float waitDuration = 5f;

    protected override void Init()
    {
        buttons = new SimonButton[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            buttons[i] = parts[i] as SimonButton;
        }

        vowelKeyTable = new SimonButton[3, 4]
        {
            { buttons[1], buttons[0], buttons[3], buttons[2] }, // Blue, Red, Yellow, Green
            { buttons[3], buttons[2], buttons[1], buttons[0] }, // Yellow, Green, Blue, Red
            { buttons[2], buttons[0], buttons[3], buttons[1] } // Green, Red, Yellow, Blue
        };
        nVowelKeyTable = new SimonButton[3, 4]
        {
            { buttons[1], buttons[3], buttons[2], buttons[0] }, // Blue, Yellow, Green, Red
            { buttons[0], buttons[1], buttons[3], buttons[2] }, // Red, Blue, Yellow, Green
            { buttons[3], buttons[2], buttons[1], buttons[0] } // Yellow, Green, Blue, Red
        };

        flashOrder = RandomUtil.GetShuffled(buttons);

        bomb.OnBombStrike += SetKeyEvent;

        StartCoroutine(FlashRoutine_Coroutine());
    }
    protected override void SetKeyEvent()
    {
        int strikes = bomb.CurStrike;
        SimonButton[,] keyTable;
        if (bomb.HasSerialVowel())
        {
            keyTable = vowelKeyTable;
        }
        else
        {
            keyTable = nVowelKeyTable;
        }

        int tableIndex = Array.IndexOf(buttons, flashOrder[keyCursor]);
        keyEvent.part = keyTable[strikes, tableIndex];
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
        base.Strike(info);
        ResetKeyCursor();
        SetKeyEvent();
    }

    private void ResetKeyCursor()
    {
        keyCursor = 0;
    }
}
