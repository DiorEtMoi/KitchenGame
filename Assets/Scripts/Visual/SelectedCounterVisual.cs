using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject selectedCounter;

    private BaseCounter baseCounter;

    private void Awake()
    {
        baseCounter = GetComponent<BaseCounter>();
    } 
    private void Start()
    {
        if(Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
        }
        else
        {
            Player.OnAnySpawnPlayer += Player_OnAnySpawnPlayer;
        }
    }

    private void Player_OnAnySpawnPlayer(object sender, System.EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged -= Instance_OnSelectedCounterChanged;
            Player.LocalInstance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
        }
    }

    private void Instance_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArg e)
    {
        if(baseCounter == e.counter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        selectedCounter.SetActive(true);
    }
    private void Hide()
    {
        selectedCounter.SetActive(false);
    }
}
