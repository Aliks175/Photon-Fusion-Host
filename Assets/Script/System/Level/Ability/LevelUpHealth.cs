public class LevelUpHealth : ILevelUps
{
    private int _valueHP = 1;
    private int _minLevel = 3;

    public LevelUpHealth(int valueHp, int minLevel)
    {
        _valueHP = valueHp;
        _minLevel = minLevel;
    }

    public void LevelUp(CharacterData systemLevel)
    {
        if (systemLevel.SystemLevel.Level >= _minLevel)
        {
            HPUP(systemLevel);
        }
    }

    private void HPUP(CharacterData playerCharecter)
    {
        PlayerHealth playerHealth = playerCharecter.PlayerHealth;

        playerHealth.ChangeMaxHealth(playerHealth.MaxHealth + _valueHP);
    }
}
