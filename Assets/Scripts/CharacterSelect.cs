using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelect : NetworkBehaviour
{
    public static CharacterSelect instance { get; private set; }
    private Dictionary<ulong, bool> playerReadyDict;
    public event EventHandler OnPlayerReady;
    private void Awake()
    {
        instance = this;
        playerReadyDict = new Dictionary<ulong, bool>();
    }

    public void SetPlayerRpc()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        playerReadyDict[serverRpcParams.Receive.SenderClientId] = true;

        bool isReadyAll = true;
        foreach (ulong cliendID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDict.ContainsKey(cliendID) || !playerReadyDict[cliendID])
            {
                isReadyAll = false;
                break;
            }
        }
        if (isReadyAll)
        {
            Loader.LoadNetworkScene(Loader.Scene.GameScene);
        }
    }
    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientID)
    {
        playerReadyDict[clientID] = true;

        OnPlayerReady?.Invoke(this, EventArgs.Empty);
    }
    public bool isPlayerReady(ulong clientId)
    {
        return playerReadyDict.ContainsKey(clientId) && playerReadyDict[clientId];
    }
}
