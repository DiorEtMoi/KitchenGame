using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button clostBtn;
    [SerializeField] private Button createPubBtn;
    [SerializeField] private Button createPrivateBtn;
    [SerializeField] private TMP_InputField lobbyName;


    private void Awake()
    {
        createPubBtn.onClick.AddListener(() =>
        {
            KitchenGameLobby.instance.CreateLobby(lobbyName.text, false);
        });
        createPrivateBtn.onClick.AddListener(() =>
        {
            KitchenGameLobby.instance.CreateLobby(lobbyName.text, true);
        });
        clostBtn.onClick.AddListener(() =>
        {
            Hide();
        });
    }
    private void Start()
    {
        Hide();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
