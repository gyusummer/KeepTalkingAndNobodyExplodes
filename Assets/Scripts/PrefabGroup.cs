using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabGroup", menuName = "ScriptableObjects/PrefabGroup")]
public class PrefabGroup : ScriptableObject
{
    public GameObject[] prefabs;
}
