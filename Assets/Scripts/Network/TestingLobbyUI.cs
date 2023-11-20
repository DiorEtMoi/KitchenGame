using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour
{
    [SerializeField] private Button createBtn;
    [SerializeField] private Button joinBtn;

    private void Awake()
    {
        createBtn.onClick.AddListener(() =>
        {
            KitchenObjectNetworkManager.instance.StartHost();
            Loader.LoadNetworkScene(Loader.Scene.CharacterScene);
        });
        joinBtn.onClick.AddListener(() =>
        {
            KitchenObjectNetworkManager.instance.StartClient();
        });
    }
}
