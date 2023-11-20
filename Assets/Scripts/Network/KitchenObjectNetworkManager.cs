using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenObjectNetworkManager : NetworkBehaviour
{
    public const string PLAYER_KEY_NAME = "PlayerRefName";
    public const int MAX_PLAYER = 4;
    public static KitchenObjectNetworkManager instance { get; private set; }

    [SerializeField] private KitchenObjectListSO listKitchenObjectSO;
    [SerializeField] private List<Color> listColor;
    public event EventHandler OnTryToJoinLobby;
    public event EventHandler OnFailedConnectLobby;

    private NetworkList<PlayerData> listPlayerData;
    public event EventHandler OnListDataPlayerChanged;

    private string playerName;
    private void Awake()
    {
        instance = this;
        playerName = PlayerPrefs.GetString(PLAYER_KEY_NAME,"Player Name " + UnityEngine.Random.Range(100,10000));
        listPlayerData = new NetworkList<PlayerData>();
        listPlayerData.OnListChanged += ListPlayerData_OnListChanged;
        DontDestroyOnLoad(gameObject);

    }
    public string getPlayerName()
    {
        return playerName;
    }
    public void setPlayerName(string playerName)
    {
        this.playerName = playerName;
        PlayerPrefs.SetString(PLAYER_KEY_NAME, playerName);
    }
    private void ListPlayerData_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnListDataPlayerChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApproval;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientID)
    {
        for(int i = 0; i < listPlayerData.Count; i++)
        {
            PlayerData playerData = listPlayerData[i];
            if(playerData.clientID == clientID)
            {
                listPlayerData.Remove(playerData);
                return;
            }
        }
    }

    private void Singleton_OnClientConnectedCallback(ulong clientId)
    {
        listPlayerData.Add(new PlayerData
        {
            clientID = clientId,
            colorId = GetFirstUnuseColor()
        });
        SetPlayerNameServerRpc();
    }

    private void NetworkManager_ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if(SceneManager.GetActiveScene().name != Loader.Scene.CharacterScene.ToString()) 
        {
            response.Approved = false;
            response.Reason = "You cant join the lobby";
            return;

        }
        if(NetworkManager.Singleton.ConnectedClients.Count >= MAX_PLAYER) 
        {
            response.Approved = false;
            response.Reason = "Game is Full Player You can not join !";
            return;
        }
        response.Approved = true;
    }

    public void StartClient()
    {
        OnTryToJoinLobby?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
        NetworkManager.Singleton.StartClient();
    }

    private void Singleton_OnClientDisconnectCallback(ulong obj)
    {
        OnFailedConnectLobby?.Invoke(this, EventArgs.Empty);
    }
    private void NetworkManager_Client_OnClientConnectedCallback(ulong clientID)
    {
        SetPlayerNameServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(ServerRpcParams serverRpcParams= default)
    {
        int playerDataIndex = GetPlayerIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = listPlayerData[playerDataIndex];

        playerData.playerName = this.playerName;

        listPlayerData[playerDataIndex] = playerData;
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        int index = GetIndexFromListKitchenObjectSO(kitchenObjectSO);
        SpawnKitchenObjectServerRpc(index, parent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference parent)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObnjectSoFromIndex(kitchenObjectSOIndex);

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.perfap);

        NetworkObject kitchenObjectNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();

        kitchenObjectNetworkObject.Spawn(true);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        parent.TryGet(out NetworkObject networkObject);

        IKitchenObjectParent kitchenObjectParent =  networkObject.GetComponent<IKitchenObjectParent>();

        kitchenObject.setKitchenObjectParent(kitchenObjectParent);

    }
    
    public int GetIndexFromListKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        return listKitchenObjectSO.listKitchenObjectSO.IndexOf(kitchenObjectSO);
    }
    public KitchenObjectSO GetKitchenObnjectSoFromIndex(int index)
    {
        return listKitchenObjectSO.listKitchenObjectSO[index];
    }
    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectReference)
    {
        kitchenObjectReference.TryGet(out NetworkObject networkObject);
        KitchenObject kitchenObject = networkObject.GetComponent<KitchenObject>();

        ClearKitchenObjectInParentClientRpc(kitchenObjectReference);

        kitchenObject.DestroySelf();

    }
    [ClientRpc]
    private void ClearKitchenObjectInParentClientRpc(NetworkObjectReference kitchenObjectReference)
    {
        kitchenObjectReference.TryGet(out NetworkObject networkObject);
        KitchenObject kitchenObject = networkObject.GetComponent<KitchenObject>();

        kitchenObject.ClearObjectOnParent();
    }
    public bool IsPlayerIndexConnected(int indexPlayer)
    {
        return indexPlayer < listPlayerData.Count;
    }
    public PlayerData GetPlayerData(int playerDataIndex)
    {
        return listPlayerData[playerDataIndex];
    }
    public Color GetPlayerColor(int colorId)
    {
        return listColor[colorId];
    }
    public int GetPlayerIndexFromClientId(ulong ClientID)
    {
      for(int i = 0;  i < listPlayerData.Count; i++)
        {
            if (listPlayerData[i].clientID == ClientID)
            {
                return i;
            }
        }
      return -1;
    }
   
    public PlayerData GetPlayerDataFromClientId(ulong ClientID)
    {
        foreach (PlayerData playerData in listPlayerData)
        {
            if(playerData.clientID == ClientID)
            {
                return playerData;
            }
        }
        return default;
    }

    public PlayerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }
    public void ChangePlayerColor(int colorID)
    {
        ChangePlayerColorServerRpc(colorID);
    }
    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams= default)
    {
        if(!IsColorAvaiable(colorId))
        {
            return;
        }
        int playerDataIndex = GetPlayerIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = listPlayerData[playerDataIndex];

        playerData.colorId = colorId;

        listPlayerData[playerDataIndex] = playerData;
    }
    private bool IsColorAvaiable(int colorID)
    {
        foreach(PlayerData playerData in listPlayerData)
        {
            if(playerData.colorId == colorID)
            {
                return false;
            }
        }
        return true;
    }
    public int GetFirstUnuseColor()
    {
        for(int i = 0; i < listColor.Count; i++)
        {
            if (IsColorAvaiable(i))
            {
                return i;
            }
        }
        return -1;
    }
}
