using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    private void Start()
    {
        KitchenObjectNetworkManager.instance.OnTryToJoinLobby += Instance_OnTryToJoinLobby;
        KitchenObjectNetworkManager.instance.OnFailedConnectLobby += Instance_OnFailedConnectLobby;
        Hide();
    }

    private void Instance_OnFailedConnectLobby(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Instance_OnTryToJoinLobby(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        KitchenObjectNetworkManager.instance.OnTryToJoinLobby -= Instance_OnTryToJoinLobby;
        KitchenObjectNetworkManager.instance.OnFailedConnectLobby -= Instance_OnFailedConnectLobby;
    }
}
