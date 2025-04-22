using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageGroupSAO", menuName = "ScriptableObjects/StageGroupSAO")]
public class StageGroupSAO : ScriptableObject
{
    public string SectionTitle;
    public List<StageInfoSAO> StageInfos;
    
    public StageInfoSAO GetStageInfoOrNull(string key)
    {
        foreach (var stageInfo in StageInfos)
        {
            if (stageInfo.Key == key)
            {
                return stageInfo;
            }
        }
        return null;
    }
}
