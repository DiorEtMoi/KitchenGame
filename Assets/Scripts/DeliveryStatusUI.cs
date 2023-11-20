using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successIcon;
    [SerializeField] private Sprite failedIcon;
    [SerializeField] private Image imageTemplate;
    [SerializeField] private Image icon;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        DeliveryManager.instance.OnSuccessRecipe += Instance_OnSuccessRecipe;
        DeliveryManager.instance.OnFailedRecipe += Instance_OnFailedRecipe;
        Hide();
    }

    private void Instance_OnFailedRecipe(object sender, System.EventArgs e)
    {
        Show();
        animator.SetTrigger("Popup");
        text.text = "DELIVER FAILED";
        icon.sprite = failedIcon;
        imageTemplate.color = failedColor;
    }

    private void Instance_OnSuccessRecipe(object sender, System.EventArgs e)
    {
        Show();
        animator.SetTrigger("Popup");
        text.text = "DELIVER SUCCESS";
        icon.sprite = successIcon;
        imageTemplate.color = successColor;
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
