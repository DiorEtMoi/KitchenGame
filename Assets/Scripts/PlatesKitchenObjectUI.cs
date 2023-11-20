using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesKitchenObjectUI : MonoBehaviour
{
    private PlatesKitchenObject platesKitchenObject;
    private Transform iconTemplate; 
    private void Awake()
    {
        platesKitchenObject = GetComponentInParent<PlatesKitchenObject>();
        iconTemplate = transform.Find("item").transform;
        iconTemplate.gameObject.SetActive(false);
    }
    public void Start()
    {
        platesKitchenObject.OnAddItemToPlates += PlatesKitchenObject_OnAddItemToPlates;
    }

    private void PlatesKitchenObject_OnAddItemToPlates(object sender, PlatesKitchenObject.AddKitchenObjectSO e)
    {
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        foreach(Transform child in transform)
        {
            if (child != null)
            {
                if (child == iconTemplate) continue;
                Destroy(child.gameObject);
            }
        }
        foreach(KitchenObjectSO kitchenObjectSO in platesKitchenObject.getListKitchenObject())
        {
            Transform icontranform = Instantiate(iconTemplate, transform);
            icontranform.GetComponent<IconTemplate>().SetIconTemplate(kitchenObjectSO);
            icontranform.gameObject.SetActive(true);
        }
    }
}
