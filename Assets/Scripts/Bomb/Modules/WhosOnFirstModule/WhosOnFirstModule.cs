using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class WhosOnFirstModule : DisarmableModule
{
    private static string[] displayLabels =
    {
        "YES", "FIRST", "DISPLAY", "OKAY", "SAYS", "NOTHING", string.Empty, "BLANK", "NO", "LED", "LEAD", "READ", "RED",
        "REED", "LEED", "HOLD ON", "YOU", "YOU ARE", "YOUR", "YOU'RE", "UR", "THERE", "THEY'RE", "THEIR", "THEY ARE",
        "SEE", "C", "CEE"
    };

    private static int[] displayTable =
    {
        2, 1, 5, 1, 5, 2, 4, 3, 5, 2, 5, 3, 3, 4, 4, 5, 3, 5, 3, 3, 0, 5, 4, 3, 2, 5, 1, 5
    };

    private static string[] buttonLabels =
    {
        "READY", "FIRST", "NO", "BLANK", "NOTHING", "YES", "WHAT", "UHHH", "LEFT", "RIGHT", "MIDDLE", "OKAY", "WAIT", "PRESS", "YOU", "YOU ARE", "YOUR", "YOU'RE", "UR", "U", "UH HUH", "UH UH", "WHAT?", "DONE", "NEXT", "HOLD", "SURE", "LIKE"
    };

    private static string[][] buttonTable =
    {
        new string[]
        {
            "YES", "OKAY", "WHAT", "MIDDLE", "LEFT", "PRESS", "RIGHT", "BLANK", "READY", "NO", "FIRST", "UHHH",
            "NOTHING", "WAIT"
        },
        new string[]
        {
            "LEFT", "OKAY", "YES", "MIDDLE", "NO", "RIGHT", "NOTHING", "UHHH", "WAIT", "READY", "BLANK", "WHAT",
            "PRESS", "FIRST"
        },
        new string[]
        {
            "BLANK", "UHHH", "WAIT", "FIRST", "WHAT", "READY", "RIGHT", "YES", "NOTHING", "LEFT", "PRESS", "OKAY", "NO",
            "MIDDLE"
        },
        new string[]
        {
            "WAIT", "RIGHT", "OKAY", "MIDDLE", "BLANK", "PRESS", "READY", "NOTHING", "NO", "WHAT", "LEFT", "UHHH",
            "YES", "FIRST"
        },
        new string[]
        {
            "UHHH", "RIGHT", "OKAY", "MIDDLE", "YES", "BLANK", "NO", "PRESS", "LEFT", "WHAT", "WAIT", "FIRST",
            "NOTHING", "READY"
        },
        new string[]
        {
            "OKAY", "RIGHT", "UHHH", "MIDDLE", "FIRST", "WHAT", "PRESS", "READY", "NOTHING", "YES", "LEFT", "BLANK",
            "NO", "WAIT"
        },
        new string[]
        {
            "UHHH", "WHAT", "LEFT", "NOTHING", "READY", "BLANK", "MIDDLE", "NO", "OKAY", "FIRST", "WAIT", "YES",
            "PRESS", "RIGHT"
        },
        new string[]
        {
            "READY", "NOTHING", "LEFT", "WHAT", "OKAY", "YES", "RIGHT", "NO", "PRESS", "BLANK", "UHHH", "MIDDLE",
            "WAIT", "FIRST"
        },
        new string[]
        {
            "RIGHT", "LEFT", "FIRST", "NO", "MIDDLE", "YES", "BLANK", "WHAT", "UHHH", "WAIT", "PRESS", "READY", "OKAY",
            "NOTHING"
        },
        new string[]
        {
            "YES", "NOTHING", "READY", "PRESS", "NO", "WAIT", "WHAT", "RIGHT", "MIDDLE", "LEFT", "UHHH", "BLANK",
            "OKAY", "FIRST"
        },
        new string[]
        {
            "BLANK", "READY", "OKAY", "WHAT", "NOTHING", "PRESS", "NO", "WAIT", "LEFT", "MIDDLE", "RIGHT", "FIRST",
            "UHHH", "YES"
        },
        new string[]
        {
            "MIDDLE", "NO", "FIRST", "YES", "UHHH", "NOTHING", "WAIT", "OKAY", "LEFT", "READY", "BLANK", "PRESS",
            "WHAT", "RIGHT"
        },
        new string[]
        {
            "UHHH", "NO", "BLANK", "OKAY", "YES", "LEFT", "FIRST", "PRESS", "WHAT", "WAIT", "NOTHING", "READY", "RIGHT",
            "MIDDLE"
        },
        new string[]
        {
            "RIGHT", "MIDDLE", "YES", "READY", "PRESS", "OKAY", "NOTHING", "UHHH", "BLANK", "LEFT", "FIRST", "WHAT",
            "NO", "WAIT"
        },
        new string[]
        {
            "SURE", "YOU ARE", "YOUR", "YOU'RE", "NEXT", "UH HUH", "UR", "HOLD", "WHAT?", "YOU", "UH UH", "LIKE",
            "DONE", "U"
        },
        new string[]
        {
            "YOUR", "NEXT", "LIKE", "UH HUH", "WHAT?", "DONE", "UH UH", "HOLD", "YOU", "U", "YOU'RE", "SURE", "UR",
            "YOU ARE"
        },
        new string[]
        {
            "UH UH", "YOU ARE", "UH HUH", "YOUR", "NEXT", "UR", "SURE", "U", "YOU'RE", "YOU", "WHAT?", "HOLD", "LIKE",
            "DONE"
        },
        new string[]
        {
            "YOU", "YOU'RE", "UR", "NEXT", "UH UH", "YOU ARE", "U", "YOUR", "WHAT?", "UH HUH", "SURE", "DONE", "LIKE",
            "HOLD"
        },
        new string[]
        {
            "DONE", "U", "UR", "UH HUH", "WHAT?", "SURE", "YOUR", "HOLD", "YOU'RE", "LIKE", "NEXT", "UH UH", "YOU ARE",
            "YOU"
        },
        new string[]
        {
            "UH HUH", "SURE", "NEXT", "WHAT?", "YOU'RE", "UR", "UH UH", "DONE", "U", "YOU", "LIKE", "HOLD", "YOU ARE",
            "YOUR"
        },
        new string[]
        {
            "UH HUH", "YOUR", "YOU ARE", "YOU", "DONE", "HOLD", "UH UH", "NEXT", "SURE", "LIKE", "YOU'RE", "UR", "U",
            "WHAT?"
        },
        new string[]
        {
            "UR", "U", "YOU ARE", "YOU'RE", "NEXT", "UH UH", "DONE", "YOU", "UH HUH", "LIKE", "YOUR", "SURE", "HOLD",
            "WHAT?"
        },
        new string[]
        {
            "YOU", "HOLD", "YOU'RE", "YOUR", "U", "DONE", "UH UH", "LIKE", "YOU ARE", "UH HUH", "UR", "NEXT", "WHAT?",
            "SURE"
        },
        new string[]
        {
            "SURE", "UH HUH", "NEXT", "WHAT?", "YOUR", "UR", "YOU'RE", "HOLD", "LIKE", "YOU", "U", "YOU ARE", "UH UH",
            "DONE"
        },
        new string[]
        {
            "WHAT?", "UH HUH", "UH UH", "YOUR", "HOLD", "SURE", "NEXT", "LIKE", "DONE", "YOU ARE", "UR", "YOU'RE", "U",
            "YOU"
        },
        new string[]
        {
            "YOU ARE", "U", "DONE", "UH UH", "YOU", "UR", "SURE", "WHAT?", "YOU'RE", "NEXT", "HOLD", "UH HUH", "YOUR",
            "LIKE"
        },
        new string[]
        {
            "YOU ARE", "DONE", "LIKE", "YOU'RE", "YOU", "HOLD", "UH HUH", "UR", "SURE", "U", "WHAT?", "NEXT", "YOUR",
            "UH UH"
        },
        new string[]
        {
            "YOU'RE", "NEXT", "U", "UR", "HOLD", "DONE", "UH UH", "WHAT?", "UH HUH", "YOU", "LIKE", "SURE", "YOU ARE",
            "YOUR"
        }
    };

    private int disarmCount = 3;
    private int hitCount = 0;
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private GameObject[] hitIndicators;
    private WOFButton[] buttons;
    private WOFButton keyButton;

    protected override void Init()
    {
        buttons = new WOFButton[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            buttons[i] = parts[i] as WOFButton;
        }
    }

    protected override void SetKeyEvent()
    {
        StartCoroutine(SetRound());
    }

    private IEnumerator SetDisplayLabel(int displayLabelIndex)
    {
        string display = displayLabels[displayLabelIndex];
        displayText.text = string.Empty;
        yield return new WaitForSeconds(0.5f);
        displayText.text = display;
    }

    private IEnumerator SetRound()
    {
        int displayLabelIndex = Random.Range(0, displayLabels.Length);
        StartCoroutine(SetDisplayLabel(displayLabelIndex));
        int indexToRead = displayTable[displayLabelIndex];

        string[] labels = RandomUtil.GetRandomSubset(buttonLabels, 6);
        int buttonTableIndex = Array.IndexOf(buttonLabels, labels[indexToRead]);

        string[] stringArrayToPush = buttonTable[buttonTableIndex];

        int keyIndex = -1;
        int minStringIndex = 99;
        for (int buttonIndex = 0; buttonIndex < labels.Length; buttonIndex++)
        {
            int stringIndex = Array.IndexOf(stringArrayToPush, labels[buttonIndex]);
            if (stringIndex != -1
                && stringIndex < minStringIndex)
            {
                minStringIndex = stringIndex;
                keyIndex = buttonIndex;
            }
        }

        if (minStringIndex == 99)
        {
            Debug.Log("No Answer");
            keyIndex = Random.Range(0, buttons.Length);
            while (keyIndex == indexToRead)
            {
                keyIndex = Random.Range(0, buttons.Length);
            }

            labels[keyIndex] = stringArrayToPush[Random.Range(0, stringArrayToPush.Length)];
        }

        keyButton = buttons[keyIndex];

        int[] randomIndex = RandomUtil.GetShuffled(new int[] { 0, 1, 2, 3, 4, 5 });
        for (int i = 0; i < labels.Length; i++)
        {
            int index = randomIndex[i];
            buttons[index].SetLabel(labels[index]);
            yield return new WaitForSeconds(0.05f);
        }

        keyEvent.part = keyButton;
    }

    protected override void Hit(PartEventInfo info)
    {
        hitIndicators[hitCount].SetActive(true);
        hitCount++;
        if (hitCount >= disarmCount)
        {
            Disarm();
            return;
        }

        SetKeyEvent();
    }

    protected override void Strike(PartEventInfo info)
    {
        SetKeyEvent();
        base.Strike(info);
    }
}