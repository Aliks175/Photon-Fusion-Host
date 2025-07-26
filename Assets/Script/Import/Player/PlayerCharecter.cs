using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(CharacterData), typeof(PlayerMove), typeof(PlayerAnimation))]

public class PlayerCharecter : NetworkBehaviour
{
    public ICharacterData CharacterData { get { return _characterData; } private set { _characterData = value; } }
    [SerializeField] private float _movementSmoothingSpeed = 1f;

    private ICharacterData _characterData;
    private PlayerMove _playerMove;
    private PlayerAnimation _playerAnimation;
    private Vector3 _rawInputMovement;
    private Vector3 _smoothInputMovement;

    private Coroutine _coroutine;

    private bool _isReady = false;

    [HideInInspector] public bool SubView = false;

    public event Action OnUpdateStat;
    [Networked, HideInInspector, OnChangedRender(nameof(SendMessageSub))] public int Level { get; private set; }
    [Networked, HideInInspector, OnChangedRender(nameof(SendMessageSub))] public int Health { get; private set; }


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


        CharacterData.SystemLevel.OnLevelUp += RPC_ViewStat;
        CharacterData.PlayerHealth.OnChangeMaxHealth += RPC_ViewStat;
        Level = CharacterData.SystemLevel.Level;
        Health = CharacterData.PlayerHealth.MaxHealth;
    }


    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ViewStat()
    {
        RPC_View();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_View()
    {
        Level = CharacterData.SystemLevel.Level;
        Health = CharacterData.PlayerHealth.MaxHealth;
        SendMessageSub();
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
