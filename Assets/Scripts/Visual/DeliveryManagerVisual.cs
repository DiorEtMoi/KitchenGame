using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerVisual : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform itemTemplate;

    public void Start()
    {
        DeliveryManager.instance.OnSpawnRecipe += Instance_OnSpawnRecipe;
        DeliveryManager.instance.OnCompletedRecipe += Instance_OnCompletedRecipe;
        itemTemplate.gameObject.SetActive(false);
        UpdateVisual();
    }

    private void Instance_OnCompletedRecipe(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void Instance_OnSpawnRecipe(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        foreach (Transform t in container.transform)
        {
            if(t == itemTemplate)
            {
                continue;
            }
            Destroy(t.gameObject);
        }
        foreach(RecipeSO waitingRecipe in DeliveryManager.instance.getWaitingRecipoSO())
        {
           Transform waitingTransFrom = Instantiate(itemTemplate,container.transform);
            waitingTransFrom.GetComponent<DeliveryMangerSingleUI>().SetRecipoUI(waitingRecipe);
            waitingTransFrom.gameObject.SetActive(true);
        }
    }
}
