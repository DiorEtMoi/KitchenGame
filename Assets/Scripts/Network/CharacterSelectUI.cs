using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button mainMenu;
    [SerializeField] private Button readyBtn;
    [SerializeField] private TextMeshProUGUI lobbyNameUI;

    private void Awake()
    {
        mainMenu.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });

        readyBtn.onClick.AddListener(() =>
        {
            CharacterSelect.instance.SetPlayerRpc();
        });
    }
    private void Start()
    {
        Lobby lobby = KitchenGameLobby.instance.GetLobby();
        lobbyNameUI.text = lobby.Name;
    }
}
