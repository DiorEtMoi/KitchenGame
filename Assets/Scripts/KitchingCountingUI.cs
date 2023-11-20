using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KitchingCountingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        KitchenGameManager.instance.OnStateChanged += Instance_OnStateChanged;
        Hide();
    }
    public void Update()
    {
       text.text = Mathf.Ceil(KitchenGameManager.instance.GetCountingStartGame()).ToString();
    }

    private void Instance_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.instance.isCountingStartGame())
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
