using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataConfig", menuName = "System/Data/CharacterDataConfig")]
public class CharacterDataConfig : ScriptableObject
{
    [Min(1)] public int StartHealthPlayer;
    public LevelLine LevelLine;
    public List<ILevelUps> ListUp;


    private void OnValidate()
    {
        if (ListUp == null) { ListUp = new List<ILevelUps>(); }
        ListUp.Clear();
        ListUp.Add(new LevelUpHealth(1, 3));
    }
}
