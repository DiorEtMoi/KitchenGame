using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IKitchenObjectParent 
{

    public Transform getTopPosition();


    public void setKitchenObject(KitchenObject kitchenObject);


    public void clearKitchenObject();


    public bool HasKitchenObject();

    public NetworkObject GetNetworkObject();
}
