using Fusion;
using TMPro;
using UnityEngine;

public class VisionStatPlayer : NetworkBehaviour
{
    [SerializeField] private TextMeshPro _statTextPlayer;

    [Networked, OnChangedRender(nameof(UpdateText))] public string tempText { get; private set; }

    private PlayerCharecter _playerCharecter;
    private Transform _target;

    private void OnDisable()
    {
        _playerCharecter.OnUpdateStat -= UpdateText;
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority == false) return;
        if (_target == null) Runner.Despawn(Object);
        transform.position = _target.position;
    }

    public void Initialization(PlayerCharecter playerCharecter)
    {
        _playerCharecter = playerCharecter;
        _playerCharecter.OnUpdateStat += UpdateText;

        _target = _playerCharecter.transform;


        //_health = playerCharecter.CharacterData.PlayerHealth;
        //if (_level != null)
        //{
        //    _level.OnSetUp += (_) => SetUp();
        //    _level.OnLevelUp += UpdateText;
        //}

        //if (_health != null)
        //{
        //    _health.OnChangeMaxHealth += UpdateText;
        //}
    }

    private void SetUp()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (Object.HasStateAuthority == false) return;

        string text = $"Level : {_playerCharecter.Level}\nHP : {_playerCharecter.Health}";

        if (text != tempText)
        {
            tempText = text;
        }
        _statTextPlayer.SetText(text);
    }
}
