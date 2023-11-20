using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if(player.getKitchenObject().TryToGetPlates(out PlatesKitchenObject platesKitchenObject)) 
            {
                DeliveryManager.instance.DeliveryPlates(platesKitchenObject);
                KitchenObject.DestroyKitchenObject(player.getKitchenObject());
            }
        }
    }
}
