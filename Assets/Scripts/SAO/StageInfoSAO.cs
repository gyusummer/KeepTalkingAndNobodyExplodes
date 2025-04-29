using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageInfoSAO", menuName = "ScriptableObjects/StageInfoSAO")]
public class StageInfoSAO : ScriptableObject
{
    public string Key;
    public string Identifier;
    [TextArea]
    public string Description;
    public int LimitTimeMiniute;
    public int LimitTimeSecond;
    public int Modules;
    public int Strikes;

    public DisarmableModule[] ModuleCandidates;
}
