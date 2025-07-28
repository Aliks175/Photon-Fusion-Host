using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(CharacterData), typeof(PlayerMove), typeof(PlayerAnimation))]

public class PlayerCharecter : NetworkBehaviour
{
    [SerializeField] private GameObject _playerStat;
    private VisionStatPlayer visionStatPlayer;
    public ICharacterData CharacterData { get { return _characterData; } private set { _characterData = value; } }
    [SerializeField] private float _movementSmoothingSpeed = 1f;

    private ICharacterData _characterData;
    private PlayerMove _playerMove;
    private PlayerAnimation _playerAnimation;
    private Vector3 _rawInputMovement;
    private Vector3 _smoothInputMovement;

    private Coroutine _coroutine;

    private bool _isReady = false;

    public event Action OnUpdateStat;
    [Networked,HideInInspector, OnChangedRender(nameof(SendMessageSub))] public int Level { get; set; }
    [Networked, HideInInspector, OnChangedRender(nameof(SendMessageSub))] public int Health { get; set; }


    private void Awake()
    {
        SetupPlayerCharecter();
        _playerAnimation.SetupPlayerAnimation();

    }

    #region Photon
    public override void Spawned()
    {
        Initialization();
    }

    public override void FixedUpdateNetwork()
    {
        if (!_isReady) return;
        if (GetInput(out NetInputData data) && Runner.IsForward)
        {


            data._rawInputMovement.Normalize();
            _rawInputMovement = data._rawInputMovement;
            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();
            _playerMove.Move(Runner.DeltaTime);
            if (data.buttons.IsSet(NetInputData.Attack))
            {
                OnAttack();
            }
            if (data.buttons.IsSet(NetInputData.Defence))
            {
                OnDefence(true);
            }
            else
            {
                OnDefence(false);
            }
            UpdatePlayerAnimationMovement();
        }
    }
    #endregion

    public void Initialization()
    {
        CharacterData.Initialization(this);
        _playerMove.SetupPlayerMove();
        _isReady = true;

        ShowPlayerStat showPlayerStat = GameObject.FindFirstObjectByType<ShowPlayerStat>();
        if (showPlayerStat != null)
            showPlayerStat.Initialization(CharacterData);

        if (HasInputAuthority)
        {
            Debug.Log($"кто я - я : {Object.Name}");
        }

        CharacterData.SystemLevel.OnLevelUp += (_) => Use(_, RPC_LevelUp);
        CharacterData.PlayerHealth.OnChangeMaxHealth += (_) => Use(_, RPC_MaxHealth);
        SubShow();
        CharacterData.PlayerHealth.Initialization();
        CharacterData.SystemLevel.Initialization();
    }

    private void Use(int Value, Action<int> action)
    {
        if (HasInputAuthority)
        {
            action?.Invoke(Value);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_LevelUp(int value)
    {
        Level = value;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_MaxHealth(int value)
    {
        Health = value;
    }

    public void SubShow()
    {
        if (visionStatPlayer == null)
        {
            var b = GameObject.Instantiate(_playerStat, transform.position, Quaternion.identity);
            visionStatPlayer = b.GetComponent<VisionStatPlayer>();
            visionStatPlayer.Initialization(this);
        }
    }

    private void SendMessageSub()
    {
        OnUpdateStat?.Invoke();
    }

    private void OnAttack()
    {
        _playerAnimation.PlayAttackAnimation();
    }

    private void OnDefence(bool isDefence)
    {
        if (isDefence)
        {
            _playerMove.Speed = _playerMove._speedWalk;
            _playerAnimation.PlayDefenceAnimation(true);
        }
        else
        {
            _playerMove.Speed = _playerMove._speedRun;
            _playerAnimation.PlayDefenceAnimation(false);
        }
    }

    private void SetupPlayerCharecter()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        CharacterData = GetComponent<CharacterData>();
    }

    private void CalculateMovementInputSmoothing()
    {
        _smoothInputMovement = Vector3.Lerp(_smoothInputMovement, _rawInputMovement, Runner.DeltaTime * _movementSmoothingSpeed);
    }

    private void UpdatePlayerMovement()
    {
        _playerMove.UpdateMovementData(_smoothInputMovement);
    }

    private void UpdatePlayerAnimationMovement()
    {
        float magnitude = _smoothInputMovement.magnitude;
        if (magnitude < 0.001f) { magnitude = 0; }
        _playerAnimation.UpdateMovementAnimation(magnitude);
    }
}
