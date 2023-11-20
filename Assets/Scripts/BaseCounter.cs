using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : NetworkBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform topPosition;

    public static event EventHandler OnPlacedKitchenObject;

    private KitchenObject kitchenObject;
    public static void ResetStatic()
    {
        OnPlacedKitchenObject = null;
    }
    public virtual void Interact(Player player)
    {
        
    }
    public virtual void InteractAlternate(Player player)
    {

    }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public Transform getTopPosition()
    {
        return topPosition;
    }
    public void setKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnPlacedKitchenObject?.Invoke(this, EventArgs.Empty);
        }
    }
    public void clearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
