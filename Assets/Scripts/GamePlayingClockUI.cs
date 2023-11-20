using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerIcon;

    private void Start()
    {
        KitchenGameManager.instance.OnStateChanged += Instance_OnStateChanged;
        Hide();
    }


    private void Instance_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.instance.isGamePlaying())
        {
            Show(); 
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
        timerIcon.fillAmount = KitchenGameManager.instance.GetNormalizedPlayingGame();
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
