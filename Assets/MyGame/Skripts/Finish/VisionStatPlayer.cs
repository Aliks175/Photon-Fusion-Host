using Fusion;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VisionStatPlayer : NetworkBehaviour
{
    [SerializeField] private TextMeshPro _statTextPlayer;

    [Networked] public string tempText { get; set; }
    private ChangeDetector _changeDetector;

    private PlayerCharecter _playerCharecter;
    private Transform _target;

    private void OnDisable()
    {
        _playerCharecter.OnUpdateStat -= View;
    }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        IEnumerable<NetworkObject> data = Spawner.Instance._spawnedCharacters.Values;
        var serializedData = data.Select(@object => @object.GetComponent<PlayerCharecter>()).ToList();
        foreach (var Player in serializedData)
        {
            if (!Player.SubView)
            {
                Player.SubView = true;
                _playerCharecter = Player;
                break;
            }
        }

        if (_playerCharecter == null)
        {
            Runner.Despawn(Object);
        }
        else
        {
            Initialization();
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority == false) return;
        if (_target == null) Runner.Despawn(Object);
        transform.position = _target.position;
    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(tempText):

                    _statTextPlayer.SetText(tempText);

                    break;
            }
        }
    }

    private void Initialization()
    {
        _playerCharecter.OnUpdateStat += View;
        _target = _playerCharecter.transform;
        View();
        Debug.Log(_playerCharecter.Object.Name);
       
    }

    public void View()
    {
        if(Object.HasStateAuthority == false) return;

        string text = $"Level : {_playerCharecter.Level}\nHP : {_playerCharecter.Health}";
        tempText = text;
        _statTextPlayer.SetText(text);
    }
}
