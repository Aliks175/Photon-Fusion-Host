using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public static Spawner _instance = null;
    public static Spawner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (Spawner)FindAnyObjectByType(typeof(Spawner));
            }
            return _instance;
        }
    }

    [SerializeField] private NetworkPrefabRef _playerStat;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private InputManager inputManager;
    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    public Dictionary<PlayerRef, NetworkObject> _spawnedStat = new Dictionary<PlayerRef, NetworkObject>();
    private NetworkRunner _runner;

    private void Awake()
    {
        GameObject go = new GameObject(typeof(InputManager).Name);
        inputManager = go.AddComponent<InputManager>();

        go.transform.parent = transform;
    }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        inputManager.Initialization(_runner);
        _runner.ProvideInput = true;
        //var runnerSimulatePhysics3D = gameObject.AddComponent<RunnerSimulatePhysics3D>();
        //runnerSimulatePhysics3D.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }


    #region ButtonStartControl

    public void StartHost()
    {
        if (_runner == null)
        {
            StartGame(GameMode.Host);
        }
    }

    public void StartClient()
    {
        if (_runner == null)
        {
            StartGame(GameMode.Client);
        }
    }

    #endregion

    #region InputPlayer
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //NetInputData data = new();
        //ButtonsInput buttonsInput = inputManager.GetButton();

        //data._rawInputMovement = inputManager.GetMovement();
        //data.buttons.Set(NetInputData.Defence, buttonsInput.isDeffence);
        //data.buttons.Set(NetInputData.Attack, buttonsInput.isAttack);

        //input.Set(data);
    }
    #endregion

    #region ControlPlayers
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            _spawnedCharacters.Add(player, networkPlayerObject);
            NetworkObject networkPlayerStat = runner.Spawn(_playerStat, networkPlayerObject.gameObject.transform.position, Quaternion.identity, player);
            _spawnedStat.Add(player, networkPlayerStat);


        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
            runner.Despawn(networkObject);
            _spawnedStat.Remove(player);
        }

        if (_spawnedStat.TryGetValue(player, out NetworkObject networkStat))
        {
            runner.Despawn(networkStat);
            _spawnedStat.Remove(player);
        }

    }


    //private void Initialization(PlayerCharecter playerCharecter)
    //{
    //    playerCharecter.Initialization(_camera, _inventaryPool);
    //    _showPlayerStat.Initialization(playerCharecter.CharacterData);
    //}


    #endregion

    #region IdontNow
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log($"OnConnectedToServer");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log($"OnConnectRequest");

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }



    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log($"OnInputMissing");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log($"OnObjectEnterAOI");
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }





    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log($"OnSceneLoadDone");

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log($"OnSceneLoadStart");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"Disconect - {runner.UserId}");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }



    #endregion
}
