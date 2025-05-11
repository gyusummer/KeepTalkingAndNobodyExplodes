using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageInfoSAO", menuName = "ScriptableObjects/StageInfoSAO")]
public class StageInfoSAO : ScriptableObject
{
    public string Key;
    public string Identifier;
    [TextArea] public string Description;
    public float LimitTimeSecond;
    public int Modules;
    public int Strikes;

    public ModuleGroupSAO ModuleCandidates;

    public int LimitMinute => (int)LimitTimeSecond / 60;
    public int LimitSecond => (int)LimitTimeSecond % 60;

    public BombInfo BombInfo => new BombInfo
    {
        LimitTime = TimeSpan.FromSeconds(LimitTimeSecond),
        ModuleCount = Modules,
        StrikeCount = Strikes
    };
}