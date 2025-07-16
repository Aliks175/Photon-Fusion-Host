using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SimulationBehaviour, INetworkRunnerCallbacks
{
    // наш С# класс действий хранит в себе все наши карты действий 
    private MyInputActions _playerInput;
    private NetworkRunner _networkRunner;

    // здесь мы кэшируем нашу карту действий перемещения игрока 
    private MyInputActions.PlayerActions _playerActions;
    private Vector3 _rawInputMovement;

    private bool _isAttack;
    private bool _isDeffence;

    private void Awake()
    {
        _playerInput = new MyInputActions();
        _playerActions = _playerInput.Player;
    }

    private void OnDisable()
    {
        if (_networkRunner != null)
        {
            _playerActions.Disable();
            _playerActions.Move.performed -= OnMovement;
            _playerActions.Move.canceled -= OnMovement;
            _playerActions.Defence.performed -= OnDefence;
            _playerActions.Defence.canceled -= OnDefence;
            _playerActions.Attack.performed -= OnAttack;
            _networkRunner.RemoveCallbacks(this);
        }
    }

    public void Initialization(NetworkRunner networkRunner)
    {
        if (networkRunner != null)
        {
            _networkRunner = networkRunner;
            _playerActions.Enable();
            _playerActions.Move.performed += OnMovement;
            _playerActions.Move.canceled += OnMovement;
            _playerActions.Defence.performed += OnDefence;
            _playerActions.Defence.canceled += OnDefence;
            _playerActions.Attack.performed += OnAttack;
            _networkRunner.AddCallbacks(this);
        }
    }

    public Vector3 GetMovement()
    {
        return _rawInputMovement;
    }

    public ButtonsInput GetButton()
    {
        bool isAttacked = _isAttack;
        if (isAttacked)
        {
            _isAttack = false;
        }

        return new ButtonsInput
        {
            isAttack = isAttacked,
            isDeffence = _isDeffence
        };
    }

    private void OnDefence(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isDeffence = true;
        }
        if ((context.phase == InputActionPhase.Canceled))
        {
            _isDeffence = false;
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isAttack = true;
        }
    }

    private void OnMovement(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            Vector2 inputMovement = value.ReadValue<Vector2>();
            _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
        }
        if (value.phase == InputActionPhase.Canceled)
        {
            _rawInputMovement = Vector3.zero;
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

        NetInputData data = new();
        ButtonsInput buttonsInput = GetButton();

        data._rawInputMovement = GetMovement();
        data.buttons.Set(NetInputData.Defence, buttonsInput.isDeffence);
        data.buttons.Set(NetInputData.Attack, buttonsInput.isAttack);

        input.Set(data);
    }


    #region Nothing
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }



    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
    #endregion
}

public struct ButtonsInput
{
    public bool isAttack;
    public bool isDeffence;
}