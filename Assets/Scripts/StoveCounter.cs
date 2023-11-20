using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateEvent> OnStateChanged;

    public event EventHandler<IHasProgress.ProgressBar> OnProgressChanged;

    public class OnStateEvent : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Cooked,
        Burned
    }
    [SerializeField] private StoveCounterSO[] listStove;
    [SerializeField] private BurningRecipeSO[] listBurn;


    private StoveCounterSO stoveSO;
    private BurningRecipeSO burnSO;

    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> timer = new NetworkVariable<float>(0f);
    private NetworkVariable<float> burningTimer = new NetworkVariable<float>(0f);


    private void Start()
    {
        state.Value = State.Idle;
    }
    public override void OnNetworkSpawn()
    {
        timer.OnValueChanged += FryingTimer_ValueChanged;
        burningTimer.OnValueChanged += BurningTimer_ValueChanged;
        state.OnValueChanged += State_ValueChanged;
    }
    public void FryingTimer_ValueChanged(float valuePrevious,float valueNext)
    {
        float fryingTimerMax = stoveSO != null ? stoveSO.timeToCook : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.ProgressBar
        {
            normalized = (float)timer.Value / fryingTimerMax
        });
    }
    public void BurningTimer_ValueChanged(float valuuePrevious, float valuueNext)
    {
        float burningTimerMax = burnSO != null ? burnSO.burningTimer : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.ProgressBar
        {
            normalized = (float)burningTimer.Value / burningTimerMax
        });
    }
    public void State_ValueChanged(State statePrevious, State stateNext)
    {
        OnStateChanged?.Invoke(this, new OnStateEvent
        {
            state = state.Value
        });
        if(state.Value == State.Burned || state.Value == State.Idle)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.ProgressBar
            {
                normalized = 0f
            });
        }
    }
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (HasKitchenObject())
        {
            switch (state.Value)
            {
                case State.Idle:
                    state.Value = State.Frying;
                   
                    break;
                case State.Frying:
                        timer.Value += Time.deltaTime;
                        
                        if (timer.Value > stoveSO.timeToCook)
                        {
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());

                            KitchenObjectNetworkManager.instance.SpawnKitchenObject(stoveSO.output, this);


                            SetBurningRecipeSOClientRpc(
                            KitchenObjectNetworkManager.instance.GetIndexFromListKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO())
                            );

                            burningTimer.Value = 0;
                      
                            state.Value = State.Cooked;
                        }
                    
                    break;
                case State.Cooked:
                    burningTimer.Value += Time.deltaTime;
                    
                    if (burningTimer.Value > burnSO.burningTimer)
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObjectNetworkManager.instance.SpawnKitchenObject(burnSO.output, this);


                        state.Value = State.Burned;
                        
                    }
                    
                    break;
                case State.Burned:
                    break;
            }
        }
      
    }
    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            //the counter dont have anything
            if (player.HasKitchenObject() && HasStoveRecipeSo(player.getKitchenObject().GetKitchenObjectSO()))
            {
                KitchenObject kitchenObject = player.getKitchenObject();
                kitchenObject.setKitchenObjectParent(this);


                int indexKitchenObject = KitchenObjectNetworkManager.instance.GetIndexFromListKitchenObjectSO(kitchenObject.GetKitchenObjectSO());
                InteractLoginServerRpc(indexKitchenObject);
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

                        SetStateStoveCounterServerRpc(State.Idle);


                    }
                }
            }
            else
            {
                GetKitchenObject().setKitchenObjectParent(player);

                SetStateStoveCounterServerRpc(State.Idle);
                
                            
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetStateStoveCounterServerRpc(State state)
    {
        this.state.Value = state;
    }
    [ServerRpc(RequireOwnership = false)]
    public void InteractLoginServerRpc(int indexKitchenObject)
    {
        timer.Value = 0;
        state.Value = State.Frying;
        SetFryingRecipeSOClientRpc(indexKitchenObject);
    }
    [ClientRpc]
    public void SetFryingRecipeSOClientRpc(int indexKitchenObject)
    {
        KitchenObjectSO kitchenObjectSO = KitchenObjectNetworkManager.instance.GetKitchenObnjectSoFromIndex(indexKitchenObject);
        stoveSO = GetStoveRecipeSOFromInput(kitchenObjectSO);
          
    }
    [ClientRpc]
    public void SetBurningRecipeSOClientRpc(int indexKitchenObject)
    {
        KitchenObjectSO kitchenObjectSO = KitchenObjectNetworkManager.instance.GetKitchenObnjectSoFromIndex(indexKitchenObject);
        burnSO = GetBurningRecipeSOFromInput(kitchenObjectSO);

    }
    private bool HasStoveRecipeSo(KitchenObjectSO input)
    {
        StoveCounterSO stoveSO = GetStoveRecipeSOFromInput(input);
        return stoveSO != null;

    }
    private KitchenObjectSO GetKitchenObjectFromRecipe(KitchenObjectSO input)
    {
        StoveCounterSO stoveSo = GetStoveRecipeSOFromInput(input);
        if (stoveSo != null)
        {
            return stoveSo.output;
        }
        else
        {
            return null;
        }

    }
    private StoveCounterSO GetStoveRecipeSOFromInput(KitchenObjectSO input)
    {

        foreach (StoveCounterSO stoveSo in listStove)
        {
            if (stoveSo.input == input)
            {
                return stoveSo;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOFromInput(KitchenObjectSO input)
    {

        foreach (BurningRecipeSO burnSO in listBurn)
        {
            if (burnSO.input == input)
            {
                return burnSO;
            }
        }
        return null;
    }
}
