using Fusion;
using UnityEngine;

public class CharacterData : MonoBehaviour, ICharacterData
{
    [SerializeField] private CharacterDataConfig _playerConfig;

    public PlayerHealth PlayerHealth
    {
        get
        {
            if (_playerHealth == null)
            {
                PlayerHealth = new PlayerHealth(_playerConfig.StartHealthPlayer,this);
            }
            return _playerHealth;
        }
        private set
        {
            _playerHealth = value;
        }
    }

    public SystemLevel SystemLevel
    {
        get
        {
            if (_systemLevel == null)
            {
                _systemLevel = new SystemLevel(_playerConfig.LevelLine, this);
            }
            return _systemLevel;
        }
        private set
        {
            _systemLevel = value;
        }
    }

    public InventoryPlayer Inventory => _inventory;

    public PlayerCharecter PlayerCharecter => _playerCharecter;

    private PlayerHealth _playerHealth;
    private SystemLevel _systemLevel;
    private InventoryPlayer _inventory;
    private PlayerCharecter _playerCharecter;

    public void Initialization(PlayerCharecter playerCharecter)
    {
        PlayerHealth ??= new PlayerHealth(_playerConfig.StartHealthPlayer, this);
        SystemLevel ??= new SystemLevel(_playerConfig.LevelLine, this);
        _inventory = GetComponent<InventoryPlayer>();
        _playerCharecter = playerCharecter;
        SetUp();
    }

    private void SetUp()
    {
        _inventory.Initialization(this);
        _playerHealth.Initialization();
        _systemLevel.Initialization();
    }
}

public interface ICharacterData
{
    public PlayerHealth PlayerHealth { get; }
    public SystemLevel SystemLevel { get; }
    public InventoryPlayer Inventory { get; }

    public PlayerCharecter PlayerCharecter { get; }

    public abstract void Initialization(PlayerCharecter playerCharecter);

}