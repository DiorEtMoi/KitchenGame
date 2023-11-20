using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    private IHasProgress progressUI;
    private Image bar;

    private void Awake()
    {
        progressUI = GetComponentInParent<IHasProgress>();
        bar = transform.Find("Bar").GetComponent<Image>();
    }
    private void Start()
    {
        progressUI.OnProgressChanged += CuttingCounter_OnCuttingProgressChanged;
        bar.fillAmount = 0;
        Hide();

    }

    private void CuttingCounter_OnCuttingProgressChanged(object sender, IHasProgress.ProgressBar e)
    {
        bar.fillAmount = e.normalized;
        if(bar.fillAmount == 0 || bar.fillAmount == 1 )
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
