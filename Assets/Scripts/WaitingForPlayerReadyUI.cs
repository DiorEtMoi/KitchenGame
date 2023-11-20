using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayerReadyUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.instance.OnLocalPlayerChanged += Instance_OnLocalPlayerChanged;
        KitchenGameManager.instance.OnStateChanged += Instance_OnStateChanged;
        Hide();
    }

    private void Instance_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.instance.isCountingStartGame())
        {
            Hide();
        }
    }

    private void Instance_OnLocalPlayerChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.instance.IsLocalPlayerReady())
        {
            Show();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
