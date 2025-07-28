using TMPro;
using UnityEngine;

public class VisionStatPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshPro _statTextPlayer;
    [SerializeField] private float _speed = 1;
    private PlayerCharecter _playerCharecter;
    private Transform _target;
    private bool _isReady = false;

    private void Update()
    {
        if (!_isReady) return;
        if (_target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _speed);
        }
    }

    public void Initialization(PlayerCharecter playerCharecter)
    {
        _playerCharecter = playerCharecter;
        _playerCharecter.OnUpdateStat += View;
        _target = _playerCharecter.transform;
        _isReady = true;
        View();

        var a = FindObjectsByType<PlayerCharecter>(FindObjectsSortMode.None);
        foreach (var item in a)
        {
            if (item != _playerCharecter)
            {
                item.SubShow();
            }
        }
    }

    public void View()
    {
        string tempText = $"Level : {_playerCharecter.Level}\nHP : {_playerCharecter.Health}";
        _statTextPlayer.SetText(tempText);
    }
}
