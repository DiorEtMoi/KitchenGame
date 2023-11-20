using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int characterIndex;
    [SerializeField] private GameObject readyObject;
    [SerializeField] private PlayerVisuals playerVisual;
    [SerializeField] private TextMeshPro playerName;

    private void Start()
    {
        KitchenObjectNetworkManager.instance.OnListDataPlayerChanged += Instance_OnListDataPlayerChanged;
        CharacterSelect.instance.OnPlayerReady += Instance_OnPlayerReady;
        UpdateVisual();
    }

    private void Instance_OnPlayerReady(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void Instance_OnListDataPlayerChanged(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        if(KitchenObjectNetworkManager.instance.IsPlayerIndexConnected(characterIndex))
        {
            Show();
            PlayerData playerData = KitchenObjectNetworkManager.instance.GetPlayerData(characterIndex);
            readyObject.SetActive(CharacterSelect.instance.isPlayerReady(
                playerData.clientID
                ));
            playerVisual.SetColor(
                KitchenObjectNetworkManager.instance.GetPlayerColor(playerData.colorId));
            playerName.text = playerData.playerName.ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        KitchenObjectNetworkManager.instance.OnListDataPlayerChanged -= Instance_OnListDataPlayerChanged;
    }
}
