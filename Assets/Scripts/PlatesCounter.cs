using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private int maxPlates = 4;
    private int currentPlates = 0;
    private float currentTime = 0;
    [SerializeField]
    private int timeToSpawn;

    public event EventHandler OnSpawnPlated;
    public event EventHandler OnTakeAPlated;
    private void Update()
    {
        if(!IsServer)
        {
            return;
        }
        currentTime += Time.deltaTime;
        if(currentTime >= timeToSpawn && currentPlates < maxPlates)
        {
            SpawnPlatesKitchenObjectClientRpc();
        }
    }
    [ClientRpc]
    public void SpawnPlatesKitchenObjectClientRpc()
    {
        currentPlates++;
        currentTime = 0;

        OnSpawnPlated?.Invoke(this, EventArgs.Empty);
    }
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject() && currentPlates > 0)
        {
            KitchenObjectNetworkManager.instance.SpawnKitchenObject(kitchenObjectSO, player);

            InteractServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void InteractServerRpc()
    {
        InteractClientRpc();
    }
    [ClientRpc]
    public void InteractClientRpc()
    {
        currentPlates--;
        OnTakeAPlated?.Invoke(this, EventArgs.Empty);
    }
}
