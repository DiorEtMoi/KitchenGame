using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnDropKitchenObject;
    new public static void ResetStatic()
    {
        OnDropKitchenObject = null;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObjectNetworkManager.instance.DestroyKitchenObject(player.getKitchenObject());
            OnDropKitchenObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
