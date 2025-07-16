using UnityEngine;

public class AbilityItemAddMaxHealth : AbilityItem
{
    [SerializeField] private int _valueAddMaxHealth;
    public override void UseAbility(ICharacterData characterData)
    {
        _valueAddMaxHealth = Mathf.Abs(_valueAddMaxHealth);
        int newMaxHealth = characterData.PlayerHealth.MaxHealth + _valueAddMaxHealth;
        characterData.PlayerHealth.ChangeMaxHealth(newMaxHealth);
        characterData.PlayerHealth.FullHealth();
        Debug.Log($"PlayerHealth {characterData.PlayerHealth.MaxHealth}");
    }
}
