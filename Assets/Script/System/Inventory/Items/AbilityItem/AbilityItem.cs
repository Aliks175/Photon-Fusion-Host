using UnityEngine;

public abstract class AbilityItem : MonoBehaviour
{
    protected ICharacterData _characterData;
    public abstract void UseAbility(ICharacterData characterData);
   
    public virtual void Initialization(ICharacterData characterData)
    {
        _characterData = characterData;
    }
}
