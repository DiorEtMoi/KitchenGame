using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconTemplate : MonoBehaviour
{
    [SerializeField] private Image icon;

   
    public void SetIconTemplate(KitchenObjectSO kitchenObjectSO)
    {
        icon.sprite = kitchenObjectSO.icon;
    }
}
