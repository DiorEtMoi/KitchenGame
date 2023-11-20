using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnGrabContainer;

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            KitchenObjectNetworkManager.instance.SpawnKitchenObject(kitchenObjectSO, player);
            InteractServerRpc();
        }
    }
    [ServerRpc (RequireOwnership = false)]
   public void InteractServerRpc()
    {
        InteractClientRpc();
    }
    [ClientRpc]
    public void InteractClientRpc()
    {
        OnGrabContainer?.Invoke(this, EventArgs.Empty);
    }
}
