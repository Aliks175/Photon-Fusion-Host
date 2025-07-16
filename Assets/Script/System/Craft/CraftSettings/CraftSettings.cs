using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftSettings", menuName = "System/Craft/CraftSettings")]
public class CraftSettings : ScriptableObject
{
    public List<CraftCombination> CraftCombinations;

    /// <summary>
    /// Сортировка списков крафта
    /// </summary>
    public void Initialization()
    {
        if (CraftCombinations == null) return;
        foreach (var item in CraftCombinations)
        {
            item.Source.Sort();
        }
    }
}


[Serializable]
public struct CraftCombination
{
    public List<string> Source;
    public GameObject Result;
}