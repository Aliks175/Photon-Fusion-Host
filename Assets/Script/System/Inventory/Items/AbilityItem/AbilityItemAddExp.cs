using UnityEngine;

public class AbilityItemAddExp : AbilityItem
{
    [SerializeField] private int _exp;
    public override void UseAbility(ICharacterData characterData)
    {
        characterData.SystemLevel.SetEx(_exp);
    }
}
