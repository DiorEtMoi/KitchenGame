using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractToReadyUI : MonoBehaviour
{

    private void Start()
    {
        KitchenGameManager.instance.OnLocalPlayerChanged += Instance_OnLocalPlayerChanged;
        Show();
    }

    private void Instance_OnLocalPlayerChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.instance.IsLocalPlayerReady())
        {
            Hide();
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
