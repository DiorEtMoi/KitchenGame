using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryMangerSingleUI : MonoBehaviour
{
    [SerializeField]private Transform iconTemplate;
    [SerializeField]private Transform itemContainer;

    [SerializeField]private TextMeshProUGUI recipeName;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipoUI(RecipeSO recipoSO)
    {
        recipeName.text = recipoSO.RecipeName;
        foreach (Transform t in itemContainer.transform)
        {
            if(t == iconTemplate)
            {
                continue;
            }
            Destroy(t.gameObject);
        }
        foreach (KitchenObjectSO item in recipoSO.recipes)
        {
            Transform itemTransfrom = Instantiate(iconTemplate, itemContainer);
            itemTransfrom.GetComponent<Image>().sprite = item.icon;
            itemTransfrom.gameObject.SetActive(true);
        }
    }
   


}
