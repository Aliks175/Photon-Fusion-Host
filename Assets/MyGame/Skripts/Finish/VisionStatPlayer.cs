using Fusion;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VisionStatPlayer : NetworkBehaviour
{
    [SerializeField] private TextMeshPro _statTextPlayer;

    [Networked, OnChangedRender(nameof(View))] public string tempText { get; set; }

    //private ChangeDetector _changeDetector;

    private PlayerCharecter _playerCharecter;
    private Transform _target;

    private void OnDisable()
    {
        _playerCharecter.OnUpdateStat -= View;
    }

    public override void Spawned()
    {
        var player = Object.InputAuthority;
        //_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if (Spawner.Instance._spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            _playerCharecter = networkObject.GetComponent<PlayerCharecter>();
        }

        if (_playerCharecter == null)
        {
            Runner.Despawn(Object);
        }
        else
        {
            IEnumerable<NetworkObject> data = Spawner.Instance._spawnedStat.Values;
            var serializedData = data.Select(@object => @object.GetComponent<VisionStatPlayer>()).ToList();
            if (serializedData.Count > 0)
            {
                foreach (var Player in serializedData)
                {
                    Player.Initialization();
                }
            }
            Initialization();
            
        }
    }

    public override void FixedUpdateNetwork()
    {
        _statTextPlayer.SetText(tempText);
        if (Object.HasStateAuthority == false) return;
        if (_target == null) Runner.Despawn(Object);
        transform.position = _target.position;
    }

    //public override void Render()
    //{
    //    foreach (var change in _changeDetector.DetectChanges(this))
    //    {
    //        switch (change)
    //        {
    //            case nameof(tempText):

    //                _statTextPlayer.SetText(tempText);

    //                break;
    //        }
    //    }
    //}

    public void Initialization()
    {
        //if (Object.HasStateAuthority == false) return;

        _playerCharecter.OnUpdateStat += View;



        _target = _playerCharecter.transform;

        View();

        //_statTextPlayer.SetText(_playerCharecter.Object.Name);
        //View();
        //Debug.Log(_playerCharecter.Object.Name);

    }

    public void View()
    {
        tempText = $"Level : {_playerCharecter.Level}\nHP : {_playerCharecter.Health}";
        //string text = $"Level : {_playerCharecter.Level}\nHP : {_playerCharecter.Health}";
        //tempText = text;
        //tempText = $"Load {gameObject.name}";
        _statTextPlayer.SetText(tempText);
    }
}
