using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button backBtn;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(() =>
        {
            KitchenGameManager.instance.OnPauseGameFunc();
        });
        backBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);

        });
    }
    private void Start()
    {
        KitchenGameManager.instance.OnPauseGame += Instance_OnPauseGame;
        Hide();
    }

    private void Instance_OnPauseGame(object sender, KitchenGameManager.TogglePause e)
    {
        if (e.toggle)
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
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
