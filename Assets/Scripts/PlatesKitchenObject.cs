using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSO;
    [SerializeField]
    private List<KitchenObjectSO> kitchenObjectsSO;

    public event EventHandler<AddKitchenObjectSO> OnAddItemToPlates;
    public class AddKitchenObjectSO : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    protected override void Awake()
    {
        base.Awake();
        kitchenObjectsSO = new List<KitchenObjectSO>();
    }
    public bool AddItemToPlates(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSO.Contains(kitchenObjectSO))
        {
            return false;
        }
        if (kitchenObjectsSO.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            AddItemToPlatesServerRpc(
                KitchenObjectNetworkManager.instance.GetIndexFromListKitchenObjectSO(kitchenObjectSO)
                );
            return true;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void AddItemToPlatesServerRpc(int indexKitchenObject)
    {
        AddItemToPlatesClientRpc(indexKitchenObject);
    }
    [ClientRpc]
    public void AddItemToPlatesClientRpc(int indexKitchenObject)
    {
        KitchenObjectSO kitchenObjectSO = KitchenObjectNetworkManager.instance.GetKitchenObnjectSoFromIndex(indexKitchenObject);
        kitchenObjectsSO.Add(kitchenObjectSO);
        OnAddItemToPlates?.Invoke(this, new AddKitchenObjectSO
        {
            kitchenObjectSO = kitchenObjectSO
        });     
    }

    public List<KitchenObjectSO> getListKitchenObject()
    {
        return kitchenObjectsSO;
    }
}
