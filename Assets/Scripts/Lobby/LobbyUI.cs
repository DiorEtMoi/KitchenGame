using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button createLobbyBtn;
    [SerializeField] private Button quickJoinBtn;
    [SerializeField] private TMP_InputField playerInputNameUI;
    [SerializeField] private CreateLobbyUI createLobbyUi;


    private void Awake()
    {
        mainMenuBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
        createLobbyBtn.onClick.AddListener(() =>
        {
           createLobbyUi.Show();
        });
        quickJoinBtn.onClick.AddListener(() =>
        {
            KitchenGameLobby.instance.QuickJoin();
        });
    }
    private void Start()
    {
        playerInputNameUI.text = KitchenObjectNetworkManager.instance.getPlayerName();
        playerInputNameUI.onValueChanged.AddListener((string newText) =>
        {
            KitchenObjectNetworkManager.instance.setPlayerName(newText);
        });
    }
}
