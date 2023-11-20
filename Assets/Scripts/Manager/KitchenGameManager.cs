using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler<TogglePause> OnPauseGame;
    public event EventHandler OnLocalPlayerChanged;

    [SerializeField] private Transform playerPerfap;
    public class TogglePause : EventArgs
    {
        public bool toggle;
    }
    private enum State
    {
        waitingToStart,
        countingTOStart,
        GamePlaying,
        GameOver
    }
    private NetworkVariable<State> state = new NetworkVariable<State>(State.waitingToStart);
    private bool isLocalPlayerReady = false;

    private NetworkVariable<float> coutingTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gameStartTimer = new NetworkVariable<float>(300f);
    private bool togglePause;

    private Dictionary<ulong, bool> playerReadyDict;

    public void Awake()
    {
        instance = this;
        state.Value = State.waitingToStart;
        playerReadyDict = new Dictionary<ulong, bool>();
    }
    public void Start()
    {
        GameInput.instance.OnPauseGame += Instance_OnPauseGame;
        GameInput.instance.OnInteractCounter += Instance_OnInteractCounter;
    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += Stated_ValueChanged;
        if(IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach(ulong client in NetworkManager.Singleton.ConnectedClientsIds) 
        {
            Transform player = Instantiate(playerPerfap);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(client,true);
        }
    }

    private void Stated_ValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Instance_OnInteractCounter(object sender, EventArgs e)
    {
        if(state.Value == State.waitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerChanged?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDict[serverRpcParams.Receive.SenderClientId] = true;

        bool isReadyAll = true;
        foreach(ulong cliendID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerReadyDict.ContainsKey(cliendID) || !playerReadyDict[cliendID]) 
            {
                isReadyAll = false;
                break;
            }
        }
        if(isReadyAll)
        {
            state.Value = State.countingTOStart;
        }
    }
    private void Instance_OnPauseGame(object sender, EventArgs e)
    {
        OnPauseGameFunc();
    }

    public void Update()
    {
        if (!IsServer)
        {
            return;
        }
        switch (state.Value)
        {
            case State.waitingToStart:
                break;
            case State.countingTOStart:
                coutingTimer.Value -= Time.deltaTime;
                if (coutingTimer.Value < 0)
                {
                    state.Value = State.GamePlaying;                 
                }
                break;
            case State.GamePlaying:
                gameStartTimer.Value -= Time.deltaTime;
                if (gameStartTimer.Value < 0)
                {
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:        
                break;
        }
    }
    public float GetNormalizedPlayingGame()
    {
        return gameStartTimer.Value / 300f;
    }
    public bool isCountingStartGame()
    {
        return state.Value == State.countingTOStart;
    }
    public bool isStartGame()
    {
        return state.Value == State.GamePlaying;
    }
    public float GetCountingStartGame()
    {
        return coutingTimer.Value;
    }
    public bool isGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }
    public bool isGameOver()
    {
        return state.Value == State.GameOver;
    }
    public void OnPauseGameFunc()
    {
        togglePause = !togglePause;
        if(togglePause)
        {
            OnPauseGame?.Invoke(this, new TogglePause
            {
                toggle = togglePause
            });
        }
        else
        {
            Time.timeScale = 1f;
            OnPauseGame?.Invoke(this, new TogglePause
            {
                toggle = togglePause
            });
        }
    }
   public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
    public bool isWaitingToStart()
    {
        return state.Value == State.waitingToStart;
    }
}
