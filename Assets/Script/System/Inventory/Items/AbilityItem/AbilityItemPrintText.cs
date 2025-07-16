using UnityEngine;

public class AbilityItemPrintText : AbilityItem
{
    [SerializeField] private string _text;
    public override void UseAbility(ICharacterData characterData)
    {
        Debug.Log(_text);
    }
}
