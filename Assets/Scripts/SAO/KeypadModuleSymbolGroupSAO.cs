using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeypadModuleSymbolGroup", menuName = "ScriptableObjects/KeypadModuleSymbolGroup")]
public class KeypadModuleSymbolGroup : ScriptableObject
{
    public Texture2D[] symbols;
}
