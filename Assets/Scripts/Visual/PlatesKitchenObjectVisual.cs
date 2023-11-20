using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesKitchenObjectVisual : MonoBehaviour
{
    private PlatesKitchenObject platesKitchenObject;
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO KitchenObjectSO;
        public GameObject KitchenObject;
    }
    [SerializeField] private List<KitchenObjectSO_GameObject> listGameObject;
    private void Awake()
    {
        platesKitchenObject = GetComponentInParent<PlatesKitchenObject>();
        foreach(KitchenObjectSO_GameObject kitchen in listGameObject)
        {
            kitchen.KitchenObject.SetActive(false);
        }
    }
    public void Start()
    {
        platesKitchenObject.OnAddItemToPlates += PlatesKitchenObject_OnAddItemToPlates;
    }

    private void PlatesKitchenObject_OnAddItemToPlates(object sender, PlatesKitchenObject.AddKitchenObjectSO e)
    {
        foreach (KitchenObjectSO_GameObject kitchen in listGameObject)
        {
            if(e.kitchenObjectSO == kitchen.KitchenObjectSO)
            {
                kitchen.KitchenObject.SetActive(true);
            }
        }
    }
}
