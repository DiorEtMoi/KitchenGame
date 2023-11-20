using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO so;

    private FollowTransform followTransform;

    private IKitchenObjectParent  KitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO() { return so; }

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }

    public void setKitchenObjectParent(IKitchenObjectParent KitchenObjectParent)
    {
        setKitchenObjectParentServerRpc(KitchenObjectParent.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership = false)]
    public void setKitchenObjectParentServerRpc(NetworkObjectReference parent)
    {
        setKitchenObjectParentClientRpc(parent);
    }
    [ClientRpc]
    public void setKitchenObjectParentClientRpc(NetworkObjectReference parent)
    {
        parent.TryGet(out NetworkObject networkObject);

        IKitchenObjectParent kitchenObjectParent = networkObject.GetComponent<IKitchenObjectParent>();
        if (this.KitchenObjectParent != null)
        {
            this.KitchenObjectParent.clearKitchenObject();
        }

        this.KitchenObjectParent = kitchenObjectParent;

        this.KitchenObjectParent.setKitchenObject(this);

        followTransform.SetFollowTransform(this.KitchenObjectParent.getTopPosition());

    }
    public void DestroySelf()
    {
        Destroy(gameObject);
        
    }
    public void ClearObjectOnParent()
    {
        KitchenObjectParent.clearKitchenObject();
    }
    public bool TryToGetPlates(out PlatesKitchenObject platesKitchenObject)
    {
        if(this is PlatesKitchenObject)
        {
            platesKitchenObject = this as PlatesKitchenObject; 
            return true;
        }
        else
        {
            platesKitchenObject = null;
            return false;
        }
    }
    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent parent)
    {
        KitchenObjectNetworkManager.instance.SpawnKitchenObject(kitchenObjectSO,parent);
    }
    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenObjectNetworkManager.instance.DestroyKitchenObject(kitchenObject);
    }
}
