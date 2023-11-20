using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetWork : MonoBehaviour
{
    [SerializeField] private Button startHost;
    [SerializeField] private Button startClient;

    private void Awake()
    {
        startHost.onClick.AddListener(() =>
        {
            Debug.Log("Host");
            KitchenObjectNetworkManager.instance.StartHost();
            Hide();
        });
        startClient.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            KitchenObjectNetworkManager.instance.StartClient();
            Hide();
        });
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
