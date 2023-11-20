using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Windows;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSO;

   
    public event EventHandler OnCuttingVisual;

    public event EventHandler<IHasProgress.ProgressBar> OnProgressChanged;

    public static event EventHandler OnAnyCut;

    private int progressCutting = 0;

    new public static void ResetStatic()
    {
        OnAnyCut = null;
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //the counter dont have anything
            if (player.HasKitchenObject() && HasCuttingRecipeSo(player.getKitchenObject().GetKitchenObjectSO()))
            {
                KitchenObject kitchenObject = player.getKitchenObject();
                kitchenObject.setKitchenObjectParent(this);


                InteractLogicPlateCounterProgressServerRpc();
            }
            else
            {
                //Player dont carring anything
            }
        }
        else
        {
            //the counter having a kitchen object
            if (player.HasKitchenObject())
            {
                //Player already carring a kitchen object
                if (player.getKitchenObject().TryToGetPlates(out PlatesKitchenObject platesKitchenObject))
                {
                    if (platesKitchenObject.AddItemToPlates(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        
                    }
                }
            }
            else
            {
                GetKitchenObject().setKitchenObjectParent(player);
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void InteractLogicPlateCounterProgressServerRpc()
    {
        InteractLogicPlateCounterProgressClientRpc();
    }
    [ClientRpc]
    public void InteractLogicPlateCounterProgressClientRpc()
    {
        progressCutting = 0;


        OnProgressChanged?.Invoke(this, new IHasProgress.ProgressBar
        {
            normalized = 0
        }) ;
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasCuttingRecipeSo(GetKitchenObject().GetKitchenObjectSO()))
        {
            InteractAlternateServerRpc();
            TestingProgressCuttingDoneServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void InteractAlternateServerRpc()
    {
        InteractAlternateClientRpc();
    }
    [ClientRpc]
    public void InteractAlternateClientRpc()
    {
        CuttingRecipeSO recipeSO = GetCuttingRecipeSOFromInput(GetKitchenObject().GetKitchenObjectSO());
        progressCutting++;

        OnProgressChanged?.Invoke(this, new IHasProgress.ProgressBar
        {
            normalized = (float)progressCutting / recipeSO.cuttingProgress
        });
        OnCuttingVisual?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);
       
    }
    [ServerRpc(RequireOwnership = false)]
    public void TestingProgressCuttingDoneServerRpc()
    {
        CuttingRecipeSO recipeSO = GetCuttingRecipeSOFromInput(GetKitchenObject().GetKitchenObjectSO());
        if (progressCutting >= recipeSO.cuttingProgress)
        {

            // do something with kitchenobject
            KitchenObjectSO so = GetKitchenObjectFromRecipe(GetKitchenObject().GetKitchenObjectSO());

            KitchenObject.DestroyKitchenObject(GetKitchenObject());

            KitchenObjectNetworkManager.instance.SpawnKitchenObject(so, this);
            progressCutting = 0;
        }
    }
    private bool HasCuttingRecipeSo(KitchenObjectSO input)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOFromInput(input);
        return cuttingRecipeSO != null;
        
    }
    private KitchenObjectSO GetKitchenObjectFromRecipe(KitchenObjectSO input)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOFromInput(input);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
        
    }
    private CuttingRecipeSO GetCuttingRecipeSOFromInput(KitchenObjectSO input)
    {

        foreach (CuttingRecipeSO cuttingSo in cuttingRecipeSO)
        {
            if (cuttingSo.input == input)
            {
                return cuttingSo;
            }
        }
        return null;
    }
}
