using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CleanUpObject : MonoBehaviour
{
    private void Awake()
    {
        if(NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
        if(KitchenObjectNetworkManager.instance != null)
        {
            Destroy(KitchenObjectNetworkManager.instance.gameObject);
        }
        if(KitchenGameLobby.instance != null)
        {
            Destroy(KitchenGameLobby.instance.gameObject);

        }
    }
}
