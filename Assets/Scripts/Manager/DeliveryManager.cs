using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    public static DeliveryManager instance {  get; private set; }

    public event EventHandler OnSpawnRecipe;
    public event EventHandler OnCompletedRecipe;

    public event EventHandler OnSuccessRecipe;
    public event EventHandler OnFailedRecipe;

    private int numberDelivered;

    [SerializeField] private ListRecipeSO recipes;

    private List<RecipeSO> waitingRecipes;
    private float spawnTimer;
    private float spawnTimerMax = 4f;
    private int maxRecipes = 4;
    private void Awake()
    {
        instance = this;
        waitingRecipes = new List<RecipeSO>();
    }
    public void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (waitingRecipes.Count < maxRecipes)
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer >= spawnTimerMax)
            {
                int waitingRecipeSOIndex = UnityEngine.Random.Range(0, recipes.lisRecipeSO.Count);
                
                spawnTimer = 0;

                SpawnRecipesClientRpc(waitingRecipeSOIndex);
            }
        }
    }
    [ClientRpc]
    public void SpawnRecipesClientRpc(int waitingRecipeSOIndex)
    {
        Debug.Log("Get Recipes");
        RecipeSO recipe = recipes.lisRecipeSO[waitingRecipeSOIndex];

        waitingRecipes.Add(recipe);

        OnSpawnRecipe?.Invoke(this, EventArgs.Empty);
    }
    public void DeliveryPlates(PlatesKitchenObject platesKitchenObject)
    {
        for(int i = 0; i < waitingRecipes.Count; i++) 
        {
            RecipeSO recipeSO = waitingRecipes[i];
            if(recipeSO.recipes.Count == platesKitchenObject.getListKitchenObject().Count)
            {
                bool isMatchContentRecipeSO = true;
                foreach(KitchenObjectSO waitingRecipeSO in recipeSO.recipes)
                {
                    bool isSameKitchenObject = false;
                    foreach(KitchenObjectSO plateKitchenObject in platesKitchenObject.getListKitchenObject())
                    {
                        // has same kitchenObject 
                        if(plateKitchenObject == waitingRecipeSO)
                        {
                            isSameKitchenObject = true;
                            break;
                        }
                    }
                    if (!isSameKitchenObject)
                    {
                        isMatchContentRecipeSO = false;
                    }
                }
                if (isMatchContentRecipeSO)
                {
                    // Player delivery correct Recipe
                    DeliveryPlatesSuccessServerRpc(i);
                    return;
                }
            }
        }
        // player did not delivery correct recipe
        Debug.Log("Not Correct Recipe");
        DeliveryPlatesFailedServerRpc();


    }
    [ServerRpc(RequireOwnership = false)]
    public void DeliveryPlatesSuccessServerRpc(int recipeIndex)
    {
        DeliveryPlatesSuccessClientRpc(recipeIndex);
    }
    [ClientRpc]
    public void DeliveryPlatesSuccessClientRpc(int recipeIndex)
    {
        Debug.Log("Correct recipe");
        waitingRecipes.RemoveAt(recipeIndex);
        OnCompletedRecipe?.Invoke(this, EventArgs.Empty);
        OnSuccessRecipe?.Invoke(this, EventArgs.Empty);
        numberDelivered++;
    }
    [ServerRpc(RequireOwnership = false)]
    public void DeliveryPlatesFailedServerRpc()
    {
        DeliveryPlatesFailedClientRpc();
    }
    [ClientRpc]
    public void DeliveryPlatesFailedClientRpc()
    {
        OnFailedRecipe?.Invoke(this, EventArgs.Empty);
    }
    public List<RecipeSO> getWaitingRecipoSO()
    {
        return waitingRecipes;
    }
    public int GetNumbersDelivered()
    {
        return numberDelivered;
    }
}
