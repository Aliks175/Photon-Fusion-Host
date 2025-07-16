using Fusion;
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


    private void Awake()
    {
        SetupPlayerCharecter();
        _playerAnimation.SetupPlayerAnimation();
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


    public void Initialization(Camera camera, GameObject inventaryPool)
    {

        CharacterData.Initialization(inventaryPool, this, Runner);
        //SetUpPlayerCharecterV1_3();
        _playerMove.SetupPlayerMove(camera);
        _isReady = true;
    }
    public void UpItemTest(NetworkObject networkObject, PlayerRef player, RpcInfo info = default)
    {
        Rpc_UpItem(networkObject, player);//Вызываем Rpc внутри игрока обладающего правом на ввод InputAuthority его получает хост StateAuthority
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void Rpc_UpItem(NetworkObject networkObject, PlayerRef player, RpcInfo info = default)
    {
        Rpc_Create(networkObject, player); // Вызываем из Хоста , принимают все пользователи 
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_Create(NetworkObject networkObject, PlayerRef player, RpcInfo info = default)
    {
        if (Runner.LocalPlayer == player) // Сработает только на том пользователе который поднимал предмет 
        {

            var item = networkObject.GetComponent<GiveItem>()._uiItem;
            if (item == null) return;
            IUiItem uiItem = item.GetComponent<IUiItem>();
            if (uiItem == null) return;
            GameObject iconItem = Instantiate(item, CharacterData.Inventory._inventaryPool.transform, false); // создаем предмет в инвенторе 
            iconItem.GetComponent<IUiItem>().Initialization(_characterData);
        }
    }


    //public void OnMovement(InputAction.CallbackContext value)
    //{
    //    if (value.phase == InputActionPhase.Performed)
    //    {
    //        Vector2 inputMovement = value.ReadValue<Vector2>();
    //        _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    //    }
    //    if (value.phase == InputActionPhase.Canceled)
    //    {
    //        OnStopMove();
    //    }
    //}

    //private void OnStopMove()
    //{
    //    _rawInputMovement = Vector3.zero;
    //}

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


    //private void TakeDamage()
    //{
    //    _playerAnimation.PlayGetDamageAnimation();

    //    OnStopMove();

    //    if (_coroutine == null)
    //    {
    //        _coroutine = StartCoroutine(OnDizzy());
    //    }

    //}

    //private void OnDied()
    //{
    //    _playerAnimation.PlayDieAnimation();

    //    _coroutine = StartCoroutine(OnRespawn());
    //}

    //private IEnumerator OnRespawn()
    //{
    //    yield return new WaitForSeconds(5f);

    //    _characterData.PlayerHealth.FullHealth();
    //    //_playerHealth.Respawn();
    //    _playerAnimation.PlayRespawnAnimation();
    //    _coroutine = null;
    //}

    //private IEnumerator OnDizzy()
    //{
    //    yield return new WaitForSeconds(2f);

    //    _coroutine = null;
    //}




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
