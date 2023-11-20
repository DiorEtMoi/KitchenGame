using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //the counter dont have anything
            if (player.HasKitchenObject())
            {
                player.getKitchenObject().setKitchenObjectParent(this);
            }
            else
            {
                //Player dont carring anything
            }
        }
        else
        {
            //the counter having a kitchen object
            if(player.HasKitchenObject())
            {
                if(player.getKitchenObject().TryToGetPlates(out PlatesKitchenObject platesKitchenObject) )
                {
                    if (platesKitchenObject.AddItemToPlates(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
                else
                {
                    if(GetKitchenObject().TryToGetPlates(out platesKitchenObject))
                    {
                        if (platesKitchenObject.AddItemToPlates(player.getKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroyKitchenObject(player.getKitchenObject());
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().setKitchenObjectParent(player);
            }
        }
    }
   
}
