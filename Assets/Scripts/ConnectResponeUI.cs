using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectResponeUI : MonoBehaviour
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private TextMeshProUGUI text;
    private void Awake()
    {
        closeBtn.onClick.AddListener(Hide);
    }
    private void Start()
    {
        KitchenObjectNetworkManager.instance.OnFailedConnectLobby += Instance_OnFailedConnectLobby;
        Hide();
    }

    private void Instance_OnFailedConnectLobby(object sender, System.EventArgs e)
    {
        Show();
        text.text = NetworkManager.Singleton.DisconnectReason;
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
        KitchenObjectNetworkManager.instance.OnFailedConnectLobby -= Instance_OnFailedConnectLobby;
    }
}
