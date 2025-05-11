using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModuleGroup", menuName = "ScriptableObjects/ModuleGroupSAO")]
public class ModuleGroupSAO : ScriptableObject
{
	public DisarmableModule[] prefabs;
}
